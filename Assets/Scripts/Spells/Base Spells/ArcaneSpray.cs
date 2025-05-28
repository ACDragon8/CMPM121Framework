using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using UnityEngine;

public class ArcaneSpray : Spell
{
    public ArcaneSpray(SpellCaster owner) : base(owner) { }
    public override void SetProperties(JObject spellAttributes)
    {
        //Read and parse extra fields Json object here
        string N = spellAttributes["N"].ToString();
        n = ReversePolishCalc.CalculateFloat(ReplaceWithDigits(N));
        string spry = spellAttributes["spray"].ToString();
        if (!float.TryParse(spry, out spray))
        {
            spray = 0.5f;
        }

        projectile_path = spellAttributes["projectile"]["trajectory"].ToString();
        string spd = spellAttributes["projectile"]["speed"].ToString();
        if (!Int32.TryParse(spd, out projectile_speed))
        {
            projectile_speed = 10;
        }
        string proj_icon = spellAttributes["projectile"]["sprite"].ToString();
        if (!Int32.TryParse(proj_icon, out projectile_icon))
        {
            projectile_icon = 0;
        }
        string proj_life = spellAttributes["projectile"]["lifetime"].ToString();
        projectile_lifetime = ReversePolishCalc.CalculateFloat(ReplaceWithDigits(proj_life));
        OnHitMethod.Add(OnHit);
        base.SetProperties(spellAttributes);
    }
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        float degree_gap = spray/2;
        for (int i = 0; i < (int)n; i++)
        {
            float offset = UnityEngine.Random.Range(-degree_gap, degree_gap);
            Vector3 direction = Quaternion.Euler(offset*360, offset*360, 0) * (target - where);
            GameManager.Instance.projectileManager.CreateProjectile(
                projectile_icon, projectile_path, 
                where, direction, projectile_speed, 
                OnHitMethod, pierce, knockback, projectile_lifetime);
        }
        yield return new WaitForEndOfFrame();
    }

    public override void OnHit(Hittable other, Vector3 vector)
    {
        if (other.team != team)
        {
            other.Damage(new Damage(GetDamage(), dmgType));
        }
    }
}
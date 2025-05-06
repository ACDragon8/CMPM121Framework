using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using UnityEditor.Search;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ArcaneSpray : Spell
{
    public string projectile_path;
    public float projectile_speed;
    public int projectile_icon;
    public float projectile_lifetime;

    public float n;
    public float spray;
    public ArcaneSpray(SpellCaster owner) : base(owner) { }
    public override void SetProperties(JObject spellAttributes)
    {
        //Read and parse extra fields Json object here
        string N = spellAttributes["N"].ToString();
        n = ReversePolishCalc.CalculateFloat(ReplaceWithDigits(N));

        projectile_path = spellAttributes["projectile"]["trajectory"].ToString();
        string spd = spellAttributes["projectile"]["speed"].ToString();
        if (!float.TryParse(spd, out projectile_speed))
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
        base.SetProperties(spellAttributes);
    }
    private float GetLifeTime()
    {
        if (valueSet)
        {
            return projectile_lifetime;
        }
        return 0.1f;
    }
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        float degree_gap = 360f * spray;
        for (int i = 0; i < (int)n; i++)
        {
            Vector3 direction = (target - where) + new Vector3(Mathf.Sin(degree_gap*i), Mathf.Cos(degree_gap*i), 0);
            GameManager.Instance.projectileManager.CreateProjectile(
                projectile_icon, projectile_path, 
                where, direction, projectile_speed, 
                OnHit, GetLifeTime());
        }
        yield return new WaitForEndOfFrame();
    }

    private void OnHit(Hittable other, Vector3 vector)
    {
        if (other.team != team)
        {
            other.Damage(new Damage(GetDamage(), GetDamageType()));
        }
    }
}
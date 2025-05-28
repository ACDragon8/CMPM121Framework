using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using UnityEngine;

public class FrostNova : Spell
{
    public int slow_duration;
    public float slow_percent;
    public FrostNova(SpellCaster owner) : base(owner) { }
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
        string proj_life = spellAttributes["projectile"]["lifetime"].ToString();
        projectile_lifetime = ReversePolishCalc.CalculateFloat(ReplaceWithDigits(proj_life));

        string spd = spellAttributes["projectile"]["speed"].ToString();
        if (!int.TryParse(spd, out projectile_speed))
        {
            projectile_speed = 10;
        }
        string proj_icon = spellAttributes["projectile"]["sprite"].ToString();
        if (!int.TryParse(proj_icon, out projectile_icon))
        {
            projectile_icon = 0;
        }

        string slo_dur = spellAttributes["slow_duration"].ToString();
        if (!int.TryParse(proj_icon, out slow_duration))
        {
            slow_duration = 2;
        }
        string slo_amt = spellAttributes["slow_amount"].ToString();
        if (!float.TryParse(proj_icon, out slow_percent))
        {
            slow_percent = 1;
        }
        OnHitMethod.Add(SlowDown);
        base.SetProperties(spellAttributes);
    }
    
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        float degree_gap = spray / 2;
        for (int i = 0; i < (int)n; i++)
        {
            float offset = UnityEngine.Random.Range(-degree_gap, degree_gap);
            Vector3 direction = Quaternion.Euler(offset * 360, offset * 360, 0) * (target - where);
            GameManager.Instance.projectileManager.CreateProjectile(
                projectile_icon, projectile_path,
                where, direction, projectile_speed,
                OnHitMethod, pierce, knockback, projectile_lifetime);
        }
        yield return new WaitForEndOfFrame();
    }

    private void SlowDown(Hittable other, Vector3 vector)
    {
        if (other.team != team)
        {
            other.Damage(new Damage(1, GetDamageType()));
            EnemyController enem = other.owner.GetComponent<EnemyController>();
            enem.ModifySpeed(slow_percent, slow_duration);
        }
    }

}
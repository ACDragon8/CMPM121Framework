using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using UnityEditor.Search;
using UnityEngine;

public class ArcaneBlast : Spell
{
    
    public ArcaneBlast(SpellCaster owner) : base(owner) { }
    public override void SetProperties(JObject spellAttributes)
    {
        //Read and parse extra fields Json object here
        string N = spellAttributes["N"].ToString();
        n = ReversePolishCalc.CalculateFloat(ReplaceWithDigits(N));

        string sDmg = spellAttributes["secondary_damage"].ToString();
        secondary_damage = ReversePolishCalc.Calculate((ReplaceWithDigits(sDmg)));

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

        secondary_projectile_path = spellAttributes["secondary_projectile"]["trajectory"].ToString();
        string spd2 = spellAttributes["secondary_projectile"]["speed"].ToString();
        if (!Int32.TryParse(spd2, out secondary_projectile_speed))
        {
            secondary_projectile_speed = 10;
        }
        string lifetime = spellAttributes["secondary_projectile"]["lifetime"].ToString();
        if (!float.TryParse(lifetime, out secondary_projectile_lifetime))
        {
            secondary_projectile_lifetime = 0.1f;
        }
        string proj_icon2 = spellAttributes["secondary_projectile"]["sprite"].ToString();
        if (!Int32.TryParse(proj_icon2, out projectile_icon))
        {
            secondary_projectile_icon = 0;
        }

        base.SetProperties(spellAttributes);
    }
    
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(projectile_icon, projectile_path, where, target - where, projectile_speed, OnFirstHit);
        yield return new WaitForEndOfFrame();
    }

    private void OnFirstHit(Hittable other, Vector3 vector)
    {
        if (other.team != team)
        {
            other.Damage(new Damage(GetDamage(), GetDamageType()));
        }
        float degree_gap = 360f / n;
        for (int i = 0; i < (int) n; i++) {
            Vector3 direction = new Vector3(Mathf.Sin(degree_gap * i), Mathf.Cos(degree_gap * i), 0);
            GameManager.Instance.projectileManager.CreateProjectile(
                secondary_projectile_icon, secondary_projectile_path, 
                vector, direction, secondary_projectile_speed, OnHit, GetSecondaryLifetime());
        }
    }

    private void OnSecondHit(Hittable other, Vector3 vector) 
    {
        if (other.team != team)
        {
            other.Damage(new Damage(GetDamage(), GetDamageType()));
        }
    }
}
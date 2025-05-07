using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using UnityEngine;

public class ArcaneBolt : Spell
{
    public string projectile_path;
    public float projectile_speed;
    public int projectile_icon;
    public ArcaneBolt(SpellCaster owner) : base(owner)
    {
        this.owner = owner;
        valueSet = false;
    }

    public override void SetProperties(JObject spellAttributes)
    {
        //Read and parse extra fields Json object here
        projectile_path = spellAttributes["projectile"]["trajectory"].ToString();
        string spd = spellAttributes["projectile"]["speed"].ToString();
        projectile_speed = ReversePolishCalc.CalculateFloat(ReplaceWithDigits(spd));
        
        string proj_icon = spellAttributes["projectile"]["sprite"].ToString();
        if (!Int32.TryParse(proj_icon, out projectile_icon))
        {
            projectile_icon = 0;
        }
        base.SetProperties(spellAttributes);
    }
    public float GetProjectileSpeed() {
        if (valueSet) { 
            return projectile_speed;
        }
        return 15f;
    }
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(projectile_icon, projectile_path, where, target - where, projectile_speed, OnHit);
        yield return new WaitForEndOfFrame();
    }

    public override void OnHit(Hittable other, Vector3 vector)
    {
        //It seems like this would be modified by modifier spells.
        if (other.team != team)
        {
            other.Damage(new Damage(GetDamage(), GetDamageType()));
        }
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using UnityEngine;

public class MagicMissile : Spell
{
    public MagicMissile(SpellCaster owner) : base(owner)
    {
        this.owner = owner;
    }

    public override void SetProperties(JObject spellAttributes)
    {
        //Read and parse extra fields Json object here
        projectile_path = spellAttributes["projectile"]["trajectory"].ToString();
        string spd = spellAttributes["projectile"]["speed"].ToString();
        if (!int.TryParse(spd, out projectile_speed)) {
            projectile_speed = 10;
        }
        string proj_icon = spellAttributes["projectile"]["sprite"].ToString();
        if (!Int32.TryParse(proj_icon, out projectile_icon))
        {
            projectile_icon = 0;
        }
        base.SetProperties(spellAttributes);
    }
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(projectile_icon, projectile_path, where, target - where, projectile_speed, OnHit, pierce, knockback);
        yield return new WaitForEndOfFrame();
    }

    public override void OnHit(Hittable other, Vector3 vector)
    {
        //It seems like this would be modified by modifier spells.
        if (other.team != team)
        {
            other.Damage(new Damage(dmg, dmgType));
        }
    }
}

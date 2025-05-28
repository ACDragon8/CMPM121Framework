using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using UnityEngine;

public class StraightSlice : Spell
{
    public StraightSlice(SpellCaster owner) : base(owner) { }

    public override void SetProperties(JObject spellAttributes)
    {
        //Read and parse extra fields Json object here
        projectile_path = spellAttributes["projectile"]["trajectory"].ToString();
        string spd = spellAttributes["projectile"]["speed"].ToString();
        projectile_speed = (int) ReversePolishCalc.CalculateFloat(ReplaceWithDigits(spd));
        
        string proj_icon = spellAttributes["projectile"]["sprite"].ToString();
        if (!Int32.TryParse(proj_icon, out projectile_icon))
        {
            projectile_icon = 0;
        }

        /*string p = spellAttributes["pierce"].ToString();
        int res = 0;
        if (!int.TryParse(p, out res)) { Debug.Log("Failed to read pierce for StraightSlice"); }
        if (res == 1) { pierce = true; }
        */
        pierce = true;
        OnHitMethod.Add(OnHit);
        base.SetProperties(spellAttributes);
    }
    
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(projectile_icon, projectile_path, where, target - where, projectile_speed, OnHitMethod, pierce, knockback);
        yield return new WaitForEndOfFrame();
    }

    public override void OnHit(Hittable other, Vector3 vector)
    {
        //It seems like this would be modified by modifier spells.
        if (other.team != team)
        {
            other.Damage(new Damage(GetDamage(), dmgType));
        }
    }
}

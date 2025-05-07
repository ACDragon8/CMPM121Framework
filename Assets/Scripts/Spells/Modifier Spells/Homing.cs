using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using UnityEngine;

public class Homing : ModifierSpell
{
    public Homing(SpellCaster owner) : base(owner) { }
    public override void SetProperties(JObject spellAttributes)
    {
        string dmg_mult = spellAttributes["damage_multiplier"].ToString();
        if (!float.TryParse(dmg_mult, out damage_multiplier)) {
            Debug.Log("Failed to parse damage multiplier for Homing");
        }
        string mana_ad = spellAttributes["mana_adder"].ToString();
        if (!Int32.TryParse(mana_ad, out mana_adder))
        {
            Debug.Log("Failed to parse mana_adder for Homing");
        }
        modified_projectile_trajectory = spellAttributes["projectile_trajectory"].ToString();
        base.SetProperties(spellAttributes);
    }
    public override void ModifySpell() {
        //This way it doesn't scam folks who already have magic missile
        if (!(baseSpell.GetProjectilePath() == modified_projectile_trajectory))
        {
            baseSpell.SetDamage((int)(baseSpell.GetDamage() * damage_multiplier));
            baseSpell.SetProjectilePath(modified_projectile_trajectory);
            baseSpell.SetManaCost(baseSpell.GetManaCost() + mana_adder);
        }
    }
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        return baseSpell.Cast(where, target, team);
    }
}

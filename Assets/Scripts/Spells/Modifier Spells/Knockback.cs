using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using UnityEngine;

public class Knockback : ModifierSpell
{
    //Maybe keep an array of projectiles + their direction?
    public Knockback(SpellCaster owner) : base(owner) { }
    public override void SetProperties(JObject spellAttributes)
    {
        string mana_ad = spellAttributes["mana_adder"].ToString();
        if (!int.TryParse(mana_ad, out mana_adder))
        {
            Debug.Log("Failed to parse mana_adder for Knockback");
        }
        base.SetProperties(spellAttributes);
    }
    public override void ModifySpell() {
        baseSpell.SetManaCost(baseSpell.GetManaCost() + mana_adder);
        baseSpell.SetKnockback(true);
    }
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        return baseSpell.Cast(where, target, team);
    }
    
}

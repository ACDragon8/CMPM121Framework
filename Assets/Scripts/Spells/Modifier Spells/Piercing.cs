using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class Piercing : ModifierSpell
{
    private float lifetime;
    public Piercing(SpellCaster owner) : base(owner) { }
    public override void SetProperties(JObject spellAttributes)
    {
        string mana_mult = spellAttributes["mana_multiplier"].ToString();
        if (!float.TryParse(mana_mult, out mana_multiplier))
        {
            Debug.Log("Failed to parse mana_multiplier for Piercing");
        }
        //if (baseSpell.GetProjectileLifetime() < lifetime) { lifetime = baseSpell.GetProjectileLifetime(); }
        base.SetProperties(spellAttributes);
    }
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        return baseSpell.Cast(where, target, team);
    }
    public override void ModifySpell()
    {
        if (!baseSpell.GetPierce())
        {
            baseSpell.SetPierce(true);
            baseSpell.SetManaCost((int)(baseSpell.GetManaCost() * mana_multiplier));
        }
        else {
            Debug.Log("Can't add pierce modifier because spell already pierces");
        }
    }
}

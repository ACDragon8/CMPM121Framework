using Newtonsoft.Json.Linq;
using UnityEngine;

public class DamageAmp : ModifierSpell
{
    public DamageAmp(SpellCaster owner) : base(owner) { }
    public override void SetProperties(JObject spellAttributes)
    {
        string dmg_mult = spellAttributes["damage_multiplier"].ToString();
        if (!float.TryParse(dmg_mult, out damage_multiplier)) {
            damage_multiplier = 1;
            Debug.Log("Unable to read damage multiplier for Damage Amp");
        }

        string mana_mult = spellAttributes["mana_multiplier"].ToString();
        if (!float.TryParse(mana_mult, out mana_multiplier)) {
            mana_multiplier = 1;
            Debug.Log("Unable to read damage multiplier for Damage Amp");
        }
        base.SetProperties(spellAttributes);
        //Probably put the function here
    }
    //TODO create function that modifies damage exactly once.
}

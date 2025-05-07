using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class DamageAmp : Spell
{
    public Spell inner;
    public DamageAmp(SpellCaster owner, Spell inner) : base(owner) {
        this.inner = inner; 
     }

    
    /*
    public override void SetProperties(JObject spellAttributes)
    {
        string dmg_mult = spellAttributes["damage_multiplier"].ToString();
        if (!float.TryParse(dmg_mult, out damage_multiplier)) {
            Debug.Log("Unable to read damage multiplier for Damage Amp");
        }

        string mana_mult = spellAttributes["mana_multiplier"].ToString();
        if (!float.TryParse(mana_mult, out mana_multiplier)) {
            Debug.Log("Unable to read damage multiplier for Damage Amp");
        }
        base.SetProperties(spellAttributes);
    }
    public override void ModifySpell() {
        baseSpell.dmg = (int) (baseSpell.GetDamage() * damage_multiplier);
        baseSpell.manaCost = (int)(baseSpell.GetManaCost() * mana_multiplier);
    }*/
    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team) {
        Debug.Log("Hello World!");
        inner.Cast(where,target,team);
    } 
    
}

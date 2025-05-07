using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class Doubler : ModifierSpell
{
    public Doubler(SpellCaster owner) : base(owner) { }
    public override void SetProperties(JObject spellAttributes)
    {
        string dly = spellAttributes["delay"].ToString();
        if (!float.TryParse(dly, out delay))
        {
            Debug.Log("Unable to read delay for Doubler");
        }
        string mana_mult = spellAttributes["mana_multiplier"].ToString();
        if (!float.TryParse(mana_mult, out mana_multiplier))
        {
            Debug.Log("Unable to read mana multiplier for Doubler");
        }
        string cooldown_mult = spellAttributes["cooldown_multiplier"].ToString();
        if (!float.TryParse(cooldown_mult, out cooldown_multiplier))
        {
            Debug.Log("Unable to read cooldown multiplier for Doubler");
        }
        base.SetProperties(spellAttributes);
    }
    public override void ModifySpell()
    {
        baseSpell.manaCost = (int)(baseSpell.GetManaCost() * mana_multiplier);
        baseSpell.cooldown = (int)(baseSpell.GetCooldown() * cooldown_multiplier);
    }
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        yield return baseSpell.Cast(where, target, team);
        yield return new WaitForSeconds(delay);
        yield return baseSpell.Cast(where, target, team);
    }
}

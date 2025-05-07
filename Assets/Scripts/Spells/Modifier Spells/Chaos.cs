using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class Chaos : ModifierSpell
{
    public Chaos(SpellCaster owner) : base(owner) { }
    public override void SetProperties(JObject spellAttributes)
    {
        string dmg_mult = spellAttributes["damage_multiplier"].ToString();
        damage_multiplier = ReversePolishCalc.CalculateFloat(ReplaceWithDigits(dmg_mult));
        modified_projectile_trajectory = spellAttributes["projectile_trajectory"].ToString();
        base.SetProperties(spellAttributes);
    }
    public override void ModifySpell() {
        baseSpell.dmg = (int) (baseSpell.GetDamage() * damage_multiplier);
        baseSpell.projectile_path = modified_projectile_trajectory;
    }
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        return base.Cast(where, target, team);
    }
}

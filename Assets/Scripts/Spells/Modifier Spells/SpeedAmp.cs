using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class SpeedAmp : ModifierSpell
{
    public SpeedAmp(SpellCaster owner) : base(owner) { }
    public override void SetProperties(JObject spellAttributes)
    {
        string speed_mult = spellAttributes["speed_multiplier"].ToString();
        if (!float.TryParse(speed_mult, out speed_multiplier)) {
            Debug.Log("Unable to read speed multiplier for Speed Amp");
        }
        base.SetProperties(spellAttributes);
    }
    public override void ModifySpell() {
        baseSpell.projectile_speed = (int) (baseSpell.GetProjectileSpeed() * speed_multiplier);
    }
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        return base.Cast(where, target, team);
    }
}

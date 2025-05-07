using Newtonsoft.Json.Linq;
using UnityEngine;

public class ModifierSpell : Spell
{
    Spell baseSpell;
    public string modifier_Name;
    public string modifier_Description;
    public float damage_multiplier;
    public float mana_multiplier;
    public float speed_multiplier;
    public float delay;
    public float cooldown_multiplier;
    public int angle;
    public string modified_projectile_trajectory;
    public int mana_adder;
    public ModifierSpell(SpellCaster owner) : base(owner)
    {
        modifier_Name = "Modifier Name not set";
        modifier_Description = "Modifier Description not set";
        damage_multiplier = 1f;
        mana_multiplier = 1f;
        speed_multiplier = 1f;
        delay = 0f;
        cooldown_multiplier = 1f;
        angle = 10;
        modified_projectile_trajectory = "straight";
        mana_adder = 1;
    }
    //So maybe have set attributes set what is being modified?
    public override void SetProperties(JObject spellAttributes)
    {
        modifier_Name = spellAttributes["name"].ToString();
        modifier_Description = spellAttributes["description"].ToString();

    }
    public string GetModifierName() { return modifier_Name; }
    public string GetModifierDescription() { return modifier_Description; }
    public float GetDamageMult() { return damage_multiplier; }
    public float GetManaMult() { return mana_multiplier; }
    public float GetSpeedMult() { return speed_multiplier; }
    public float GetDelay() { return delay; }
    public float GetCoolDownMult() { return cooldown_multiplier; }
    public int GetAngle() { return angle; }
    public string GetModifiedProjectilePath() { return modified_projectile_trajectory; }
    public int GetManaAdder() { return mana_adder; }
    public void SetBaseSpell(Spell spell)
    {
        baseSpell = spell;
    }
}

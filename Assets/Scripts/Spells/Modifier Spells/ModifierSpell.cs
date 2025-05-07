using Newtonsoft.Json.Linq;
using UnityEngine;

public class ModifierSpell : Spell
{
    public Spell baseSpell;
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
    public override string GetName() { return baseSpell.name; }
    public override string Description() { return baseSpell.description; }
    public override int GetIcon() { return baseSpell.icon; }
    public override int GetManaCost() { return baseSpell.manaCost; }
    public override int GetDamage() { return baseSpell.dmg; }
    public override Damage.Type GetDamageType() { return baseSpell.dmgType; }
    public override float GetCooldown() { return baseSpell.cooldown; }
    public override string GetProjectilePath() { return baseSpell.projectile_path; }
    public override int GetProjectileSpeed() { return baseSpell.projectile_speed; }
    public override int GetProjectileIcon() { return baseSpell.projectile_icon; }
    public override float GetProjectileLifetime() { return baseSpell.projectile_lifetime; }
    public override float GetSpray() { return baseSpell.spray; }
    public override float Getn() { return baseSpell.n; }
    public override int GetSecondaryDamage() { return baseSpell.secondary_damage; }
    public override string GetSecondaryPath() { return baseSpell.secondary_projectile_path; }
    public override int GetSecondarySpeed() { return baseSpell.secondary_projectile_speed; }
    public override int GetSecondaryIcon() { return baseSpell.secondary_projectile_icon; }
    public override float GetSecondaryLifetime() { return baseSpell.secondary_projectile_lifetime; }
    public void SetBaseSpell(Spell spell)
    {
        baseSpell = spell;
        ModifySpell();
    }
    public virtual void ModifySpell() { }
}

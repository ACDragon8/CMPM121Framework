using Newtonsoft.Json.Linq;
using System;
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
    public float knockback_distance;
    public Action<Hittable, Vector3> oldOnHitMethod;
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
        knockback_distance = 0f;
    }
    //So maybe have set attributes set what is being modified?
    public override void SetProperties(JObject spellAttributes)
    {
        modifier_Name = spellAttributes["name"].ToString();
        modifier_Description = spellAttributes["description"].ToString();
    }
    public override string GetName() { return baseSpell.GetName(); }
    public override string GetDescription() { return baseSpell.GetDescription(); }
    public override int GetIcon() { return baseSpell.GetIcon(); }

    public override int GetManaCost() { return baseSpell.GetManaCost(); }
    public override void SetManaCost(int newManaCost) { baseSpell.SetManaCost(newManaCost); }

    public override int GetDamage() { return baseSpell.GetDamage(); }
    public override void SetDamage(int newDMG){ baseSpell.SetManaCost(newDMG); }

    public override Damage.Type GetDamageType() { return baseSpell.GetDamageType(); }
    public override void SetDamageType(Damage.Type newType) { baseSpell.SetDamageType(newType); }

    public override float GetCooldown() { return baseSpell.GetCooldown(); }
    public override void SetCooldown(float newCooldown) { baseSpell.SetCooldown(newCooldown); }

    public override string GetProjectilePath() { return baseSpell.GetProjectilePath(); }
    public override void SetProjectilePath(string newPath) { baseSpell.SetProjectilePath(newPath); }

    public override int GetProjectileSpeed() { return baseSpell.GetProjectileSpeed(); }
    public override void SetProjectileSpeed(int newProjSpd) { baseSpell.SetProjectileSpeed(newProjSpd); }

    public override int GetProjectileIcon() { return baseSpell.GetProjectileIcon(); }
    public override void SetProjectileIcon(int newProjIcon) { baseSpell.SetProjectileIcon(newProjIcon); }

    public override float GetProjectileLifetime() { return baseSpell.GetProjectileLifetime(); }
    public override void SetProjectileLifetime(float newProjLifetime) { baseSpell.SetProjectileLifetime(newProjLifetime); }

    public override float GetSpray() { return baseSpell.GetSpray(); }
    public override void SetSpray(float newSpray) { baseSpell.SetSpray(newSpray); }

    public override float Getn() { return baseSpell.Getn(); }
    public override void Setn(float newN) { baseSpell.Setn(newN); }

    public override int GetSecondaryDamage() { return baseSpell.GetSecondaryDamage(); }
    public override void SetSecondaryDamage(int newSecondDMG) { baseSpell.SetSecondaryDamage(newSecondDMG); }

    public override string GetSecondaryPath() { return baseSpell.GetSecondaryPath(); }
    public override void SetSecondaryPath(string newSecondPath) { baseSpell.SetSecondaryPath(newSecondPath); }

    public override int GetSecondarySpeed() { return baseSpell.GetSecondarySpeed(); }
    public override void SetSecondarySpeed(int newSecondSPD) { baseSpell.SetSecondarySpeed(newSecondSPD); }

    public override int GetSecondaryIcon() { return baseSpell.GetSecondaryIcon(); }
    public override void SetSecondaryIcon(int newSecondIcon) { baseSpell.SetSecondaryIcon(newSecondIcon); }

    public override float GetSecondaryLifetime() { return baseSpell.GetSecondaryLifetime(); }
    public override void SetSecondaryLifetime(float newSecondLifetime) { baseSpell.SetSecondaryLifetime(newSecondLifetime); }

    public override bool GetPierce() { return baseSpell.GetPierce(); }
    public override void SetPierce(bool newPierce) { baseSpell.SetPierce(newPierce); }

    public override bool GetKnockback() { return baseSpell.GetKnockback(); }
    public override void SetKnockback(bool newKnockback) { baseSpell.SetKnockback(newKnockback); }

    public void SetBaseSpell(Spell spell)
    {
        baseSpell = spell;
        ModifySpell();
    }
    public virtual void ModifySpell() { }
}

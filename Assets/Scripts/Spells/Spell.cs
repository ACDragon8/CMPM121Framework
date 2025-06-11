using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine.Rendering;

public class Spell 
{
    public float last_cast;
    public Hittable.Team team;
    public JObject spellInfo;
    //These are the spell fields below
    //This info can be found in the JSON
    public SpellCaster owner;
    public string name;
    public string description;
    public int icon;
    public int manaCost;
    public int dmg;
    public Damage.Type dmgType;
    public float cooldown;
    public string projectile_path;
    public int projectile_speed;
    public int projectile_icon;
    public float projectile_lifetime;
    public float spray;
    public float n;
    public int secondary_damage;
    public string secondary_projectile_path;
    public int secondary_projectile_speed;
    public int secondary_projectile_icon;
    public float secondary_projectile_lifetime;
    public bool knockback;
    public bool pierce;
    public List<Action<Hittable, Vector3>> OnHitMethod;

    public Spell(SpellCaster owner)
    {
        this.owner = owner;
        
        name = "Name not set";
        description = "Description not set";
        icon = 0;
        manaCost = 10;
        dmg = 10;
        dmgType = Damage.Type.ARCANE;
        cooldown = 2f;
        projectile_path = "straight";
        projectile_speed = 10;
        projectile_icon = 0;
        projectile_lifetime = 0;
        spray = 0.5f;
        n = 8;
        secondary_damage = 5;
        secondary_projectile_path = "straight";
        secondary_projectile_speed = 10;
        secondary_projectile_icon = 0;
        secondary_projectile_lifetime = 0.1f;
        pierce = false;
        knockback = false;
        OnHitMethod = new List<Action<Hittable, Vector3>>();
    }
    public virtual void SetProperties(JObject spellAttributes) 
    {
        spellInfo = spellAttributes;
        name = spellAttributes["name"].ToString();
        description = spellAttributes["description"].ToString();

        icon = spellAttributes["icon"].ToObject<int>();
        string d = spellAttributes["damage"]["amount"].ToString();
        dmg = ReversePolishCalc.Calculate(ReplaceWithDigits(d));

        dmgType = Damage.TypeFromString(spellAttributes["damage"]["type"].ToString());
        string m = spellAttributes["mana_cost"].ToString();
        if (!Int32.TryParse(m, out manaCost)) {
            Debug.Log("Failed to parse manaCost");
        }
        string c = spellAttributes["cooldown"].ToString();
        if (!float.TryParse(c, out cooldown)) {
            Debug.Log("Failed to parse cooldown");
        }
        projectile_path = spellAttributes["projectile"]["trajectory"].ToString();
    }
    protected string[] ReplaceWithDigits(string sequence) 
    {
        string[] s = sequence.Split(' ');
        int index = 0;
        foreach (string token in s) 
        {
            if (token == "wave")
            {
                s[index] = StatsManager.Instance.waveNum.ToString();
            }
            else if (token == "power") 
            {
                //We get the power value from the owner (spellcaster class passed in)
                s[index] = owner.power.ToString();
            }
            index++;
        }
        return s;
    }
    public virtual string GetName() { return name; }
    public virtual string GetDescription() { return description; }
    public virtual int GetIcon() { return icon; }

    public virtual int GetManaCost() { return manaCost; }
    public virtual void SetManaCost(int newManaCost) { manaCost = newManaCost; }

    public virtual int GetDamage()
    { //This is here so the damage numbers can update itself with the increased waves (power)
        string d = spellInfo["damage"]["amount"].ToString();
        dmg = ReversePolishCalc.Calculate(ReplaceWithDigits(d));
        return dmg;
     }
    public virtual void SetDamage(int newDMG) { dmg = newDMG; }

    public virtual Damage.Type GetDamageType() { return dmgType; }
    public virtual void SetDamageType(Damage.Type newType) { dmgType = newType; }

    public virtual float GetCooldown() { return cooldown; }
    public virtual void SetCooldown(float newCooldown) { cooldown = newCooldown; }

    public virtual string GetProjectilePath() { return projectile_path; }
    public virtual void SetProjectilePath(string newPath) { projectile_path = newPath; }

    public virtual int GetProjectileSpeed() { return projectile_speed; }
    public virtual void SetProjectileSpeed(int newProjSpd) { projectile_speed = newProjSpd; }

    public virtual int GetProjectileIcon() { return projectile_icon; }
    public virtual void SetProjectileIcon(int newProjIcon) { projectile_icon = newProjIcon; }

    public virtual float GetProjectileLifetime() { return projectile_lifetime; }
    public virtual void SetProjectileLifetime(float newProjLife) { projectile_lifetime = newProjLife; }

    public virtual float GetSpray() { return spray; }
    public virtual void SetSpray(float newSpray) { spray = newSpray; }

    public virtual float Getn() { return n; }
    public virtual void Setn(float newN) { n = newN; }

    public virtual int GetSecondaryDamage() { return secondary_damage; }
    public virtual void SetSecondaryDamage(int newSecondDMG) { secondary_damage = newSecondDMG; }

    public virtual string GetSecondaryPath() { return secondary_projectile_path; }
    public virtual void SetSecondaryPath(string newSecondPath) { secondary_projectile_path = newSecondPath; }

    public virtual int GetSecondarySpeed() { return secondary_projectile_speed; }
    public virtual void SetSecondarySpeed(int newSecondSpeed) { secondary_projectile_speed = newSecondSpeed; }

    public virtual int GetSecondaryIcon() { return secondary_projectile_icon; }
    public virtual void SetSecondaryIcon(int newSecondIcon) { secondary_projectile_icon = newSecondIcon; }

    public virtual float GetSecondaryLifetime() { return secondary_projectile_lifetime; }
    public virtual void SetSecondaryLifetime(float newSecondLifetime) { secondary_projectile_lifetime = newSecondLifetime; }

    public virtual bool GetPierce() { return pierce; }
    public virtual void SetPierce(bool newPierce) { pierce = newPierce; }

    public virtual bool GetKnockback() { return knockback; }
    public virtual void SetKnockback(bool newKnockback) { knockback = newKnockback; }

    public virtual List<Action<Hittable, Vector3>> GetOnHitMethod() { return OnHitMethod; }
    public virtual void SetOnHitMethod(Action<Hittable, Vector3> newOnHitMethod) 
    { 
        if (OnHitMethod == null) {
            OnHitMethod = new List<Action<Hittable, Vector3>>();
        }
            OnHitMethod.Add(newOnHitMethod); }
    public bool IsReady()
    {
        return (last_cast + GetCooldown() < Time.time);
    }

    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(0, "straight", where, target - where, 15f, OnHitMethod);
        yield return new WaitForEndOfFrame();
    }

    public virtual void OnHit(Hittable other, Vector3 impact)

    {
        if (other.team != team)
        {
            other.Damage(new Damage(GetDamage(), GetDamageType()));
        }
    }

}

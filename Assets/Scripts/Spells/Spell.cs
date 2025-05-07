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

    public Spell(SpellCaster owner)
    {
        //These are the default values if set attribute didn't set them
        //Debating removing this and having functions crash so it easier to find problem
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
        projectile_lifetime = 3f;
        spray = 0.5f;
        n = 8;
        secondary_damage = 5;
        secondary_projectile_path = "straight";
        secondary_projectile_speed = 10;
        secondary_projectile_icon = 0;
        secondary_projectile_lifetime = 0.1f;
    }
    public virtual void SetProperties(JObject spellAttributes) 
    {
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
    public string GetName() { return name; }
    public string Description() { return description; }
    public virtual int GetIcon() { return icon; }
    public int GetManaCost() { return manaCost; }
    public int GetDamage() { return dmg; }
    public Damage.Type GetDamageType() { return dmgType; }
    public float GetCooldown() { return cooldown; }
    public string GetProjectilePath() { return projectile_path; }
    public int GetProjectileSpeed() { return projectile_speed; }
    public int GetProjectileIcon() { return projectile_icon; }
    public float GetProjectileLifetime() { return projectile_lifetime; }
    public float GetSpray() { return spray; }
    public float Getn() { return n; }
    public int GetSecondaryDamage() { return secondary_damage; }
    public string GetSecondaryPath() { return secondary_projectile_path; }
    public int GetSecondarySpeed() { return secondary_projectile_speed; }
    public int GetSecondaryIcon() { return secondary_projectile_icon; }
    public float GetSecondaryLifetime() { return secondary_projectile_lifetime; }
    public bool IsReady()
    {
        return (last_cast + GetCooldown() < Time.time);
    }

    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(0, "straight", where, target - where, 15f, OnHit);
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;

public class Spell 
{
    public float last_cast;
    public SpellCaster owner;
    public string name;
    public string description;
    public int manaCost;
    public int dmg;
    public Damage.Type dmgType;
    public int cooldown;
    public int icon;
    public Hittable.Team team;
    public bool valueSet;

    public Spell(SpellCaster owner)
    {
        this.owner = owner;
        valueSet = false;
    }
    public virtual void SetProperties(JObject spellAttributes) 
    {
        ReversePolishCalc rpn = new ReversePolishCalc();
        icon = spellAttributes["icon"].ToObject<int>();
        string d = spellAttributes["damage"]["amount"].ToString();
        string[] dmg_split = d.Split(' ');
        foreach (string s in dmg_split) 
        {
            //TODO
        }
        dmg = rpn.Calculate(dmg_split);
        valueSet = true; 
    }
    public string GetName()
    {
        if (valueSet) { return name; } else { return "Bolt"; }  
    }
    public string Description() 
    {
        if (valueSet) { return description; } else { return "NONE"; }
    }

    public int GetManaCost()
    {
        if (valueSet) { return manaCost; } else { return 10; }
    }

    public int GetDamage()
    {
        if (valueSet) { return dmg; } 
        else { return 100; }
    }
    public Damage.Type GetDamageType()
    {
        if (valueSet) { return dmgType; } else { return Damage.Type.ARCANE; }
    }
    public float GetCooldown()
    {
        if (valueSet) { return cooldown; } else { return 0.75f; }
    }

    public virtual int GetIcon()
    {
        if (valueSet) { return icon; } else { return 0; }
    }

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

    void OnHit(Hittable other, Vector3 impact)
    {
        if (other.team != team)
        {
            other.Damage(new Damage(GetDamage(), GetDamageType()));
        }

    }

}

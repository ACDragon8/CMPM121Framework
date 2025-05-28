using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellCaster 
{
    public int mana;
    public int max_mana;
    public int mana_reg;
    public int basePower;
    public int power;
    public Hittable.Team team;
    public Spell[] spell;
    public int selectedSpell;
    private int maxSpells;

    public int spellCount;

    public SpellBuilder sb;
    public Dictionary<string, int> powerModifiers;


    public IEnumerator ManaRegeneration()
    {
        while (true)
        {
            mana += mana_reg;
            mana = Mathf.Min(mana, max_mana);
            yield return new WaitForSeconds(1);
        }
    }

    public SpellCaster(int mana, int mana_reg, int power, Hittable.Team team)
    {
        this.mana = mana;
        this.max_mana = mana;
        this.mana_reg = mana_reg;
        this.team = team;
        this.power = power;

        this.spell = new Spell[4];
        this.selectedSpell = 0;
        this.maxSpells = 4;
        this.sb = new SpellBuilder();
        this.spell[0] = sb.Build(this);
        this.spell[1] = sb.Build(this, "magic_missile");
        this.spellCount = 2;
        this.powerModifiers = new Dictionary<string, int>();
    }

    public IEnumerator Cast(Vector3 where, Vector3 target)
    {
        getPower();
        if (mana >= spell[selectedSpell].GetManaCost() && spell[selectedSpell].IsReady())
        {
            mana -= spell[selectedSpell].GetManaCost();
            yield return spell[selectedSpell].Cast(where, target, team);
        }
            yield break;
    }

    public Spell getSpell() {
        return spell[selectedSpell];
    }

    public void modifyPower(string s, int val)
    {
        if (this.powerModifiers.TryGetValue(s, out int a))
        {
            this.powerModifiers[s] = val;
        }
        else
        {
            this.powerModifiers.Add(s, val);
        }
        
    }

    public int getPower()
    {
        this.power = this.basePower;
        foreach(var (key,value) in this.powerModifiers)
        {
            this.power += value;
        }
        return this.power;
    }

    public void SetMaxMana(int val)
    {
        this.max_mana = val;
    }
    public void SetManaRegen(int val) {
        this.mana_reg = val;
    }

    public void SetSpellPower(int val) {
        this.basePower = val;
    }


    public void nextSpell()
    {
        this.selectedSpell = (this.selectedSpell + 1) % this.maxSpells;
        if (this.spell[selectedSpell] == null)
        {
            nextSpell();
        }
        EventBus.Instance.OnSwitchSpell();
    }

    public void gainMana(int val)
    {
        this.mana = Mathf.Min(this.mana + val, this.max_mana);
    }

    public bool addSpell(string spellName = "arcane_bolt")
    {
        for (int i = 0; i < this.maxSpells; i++)
        {
            if (spell[i] == null)
            {
                this.spell[i] = sb.Build(this, spellName);
                this.spellCount += 1;
                EventBus.Instance.OnSpellPickupEffect(this.spell[i], i);
                if (spellCount > 1) { EventBus.Instance.OnSpellMultipleEffect(spellCount); }
                return true;
            }
        }
        Debug.Log("Unable to add spell, all spell inventory slots full");
        return false;
    }

    public void DropSpell(int index)
    {
        if (this.spellCount <= 1)
        {
            return;
        }
        else
        {
            this.spell[index] = null;
            this.spellCount--;
            if (spellCount == 1)
            {
                EventBus.Instance.OnSpellSoloEffect(this);
            }
            if (selectedSpell == index)
            {
                nextSpell();
            }
        }
    }
}

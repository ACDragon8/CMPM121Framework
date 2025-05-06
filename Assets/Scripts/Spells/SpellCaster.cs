using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellCaster 
{
    public int mana;
    public int max_mana;
    public int mana_reg;
    public int power;
    public Hittable.Team team;
    public Spell[] spell;
    public int selectedSpell;
    private int maxSpells;


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
        this.spell[0] = new SpellBuilder().Build(this);
    }

    public IEnumerator Cast(Vector3 where, Vector3 target)
    {        
        if (mana >= spell[0].GetManaCost() && spell[0].IsReady())
        {
            mana -= spell[0].GetManaCost();
            yield return spell[0].Cast(where, target, team);
        }
        yield break;
    }

    public Spell getSpell() {
        return spell[0];
    }

    public void nextSpell() {
        this.selectedSpell = (this.selectedSpell + 1 ) % this.maxSpells;
    }

}

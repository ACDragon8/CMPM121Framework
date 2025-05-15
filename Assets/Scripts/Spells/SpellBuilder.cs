using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;


public class SpellBuilder 
{
    JObject spellList;
    public string[] spellTypes;
    public string[] modifierTypes;
    public Spell Build(SpellCaster owner, string spellName= "arcane_bolt")
    {
        
        int count = 0;
        
        string[] keywords = spellName.Split();
        foreach(var word in keywords) {
            Debug.Log(word);
            count++;
        } 

        Spell s;
        switch (keywords[count -1]) {
            case "arcane_bolt":
                s = new ArcaneBolt(owner);
                break;
            case "magic_missile":
                s = new MagicMissile(owner);
                break;
            case "arcane_blast":
                s = new ArcaneBlast(owner);
                break;
            case "arcane_spray":
                s = new ArcaneSpray(owner);
                break;
            default:
                s = new ArcaneBolt(owner);
                break;
        }
        
        
        s.SetProperties((JObject)spellList[keywords[count - 1]]);
        //Cynthia screwing around here and figuring things out
        ModifierSpell mod = new Knockback(owner);
        mod.SetProperties((JObject) spellList["knockback"]);
        mod.SetBaseSpell(s);
        /*
        ModifierSpell mod2 = new Splitter(owner);
        mod2.SetProperties((JObject)spellList["splitter"]);
        mod2.SetBaseSpell(mod);
        */
        return mod;
    }

    public string GetRandomBuild()
    {
        string s = "";
        string pick;
        IList<string> keys = spellList.Properties().Select(p => p.Name).ToList();
        int spell = Random.Range(0, (keys.Count-1));
        do {
            pick = keys[spell];
            s += pick + " " + s;
            if (spellTypes.Contains(pick)) {
                break;
            }
        } while (true);
        return s;
    }

    public SpellBuilder()
    {
        //Ideally we'd split the json into 2 files, 1 for base spells and 1 for modifiers
        //and use the keys instead of writing out the spells here
        string[] a = {"arcane_bolt", "magic_missile", "arcane_blast", "arcane_spray", "straight_slice"};
        spellTypes = a;
        string[] b = {"damage_amp", "speed_amp", "doubler", "splitter", "chaos", "homing", "knockback", "piercing"};
        modifierTypes = b;

        var spelltext = Resources.Load<TextAsset>("spells");
        JObject jo = JObject.Parse(spelltext.text);
        if (jo != null)
        {
            spellList = jo;
        }
        else 
        {
            Debug.Log("Missing spells.json");
        }
    }

}

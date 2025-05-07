using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;


public class SpellBuilder 
{
    JObject spellList;
    public string[] spellTypes;
    public Spell Build(SpellCaster owner, string spellName= "arcane_bolt")
    {
        //TODO figure out how to incorporate modifiers into this
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
        /*s.SetProperties((JObject)spellList[keywords[count -1]]);
        for (int i = 0; i < count -1; i++) {
            switch (keywords[count-1]) {
                case "damage_amp":
                Spell a = s;
                s = new DamageAmp();
                s.inner = a;
                    break;
                default:

                    break;

            }
        }

        return s;
        */
        s.SetProperties((JObject)spellList[spellName]);
        //Cynthia screwing around here and figuring things out
        ModifierSpell mod = new SpeedAmp(owner);
        mod.SetProperties((JObject) spellList["speed_amp"]);
        mod.SetBaseSpell(s);
        return mod;
    }

    public Spell RandomBuild(SpellCaster owner)
    {
        return new Spell(owner);
    }

   //So this function below is the creation function for object spell builder
   //This might be a good place to put the Json reading.
   //Dunno if we want it to be singleton or not
    public SpellBuilder()
    {
        string[] a = {"arcane_bolt", "magic_missile", "arcane_blast", "arcane_spray"};
        spellTypes = a;

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

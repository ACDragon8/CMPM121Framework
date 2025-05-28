using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Unity.VisualScripting;


public class SpellBuilder 
{
    JObject spellList;
    public string[] spellTypes;
    public string[] modifierTypes;
    public Spell Build(SpellCaster owner, string spellName= "explosive arcane_bolt")
    { 
        string[] keywords = spellName.Split();
        int count = keywords.Length;
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
            case "straight_slice":
                s = new StraightSlice(owner);
                break;
            case "frost_nova":
                s = new FrostNova(owner);
                break;
            default:
                s = new ArcaneBolt(owner);
                break;
        }
        s.SetProperties((JObject) spellList[keywords[count - 1]]);
        for(int i = 0; i < count-1; i++) {
            ModifierSpell m = null;
            switch (keywords[i]) {
                case "damage_amp":
                    m = new DamageAmp(owner);
                    break;
                case "speed_amp":
                    m = new SpeedAmp(owner);
                    break;
                case "doubler":
                    m = new Doubler(owner);
                    break;
                case "splitter":
                    m = new Splitter(owner);
                    break;
                case "chaos":
                    m = new Chaos(owner);
                    break;
                case "homing":
                    m = new Homing(owner);
                    break;
                case "knockback":
                    m = new Knockback(owner);
                    break;
                case "piercing":
                    m = new Piercing(owner);
                    break;
                case "explosive":
                    m = new Explosive(owner);
                    break;
                default:
                    break;
            }
            m.SetProperties((JObject) spellList[keywords[i]]);
            m.SetBaseSpell(s);
            s = m;
        }
        return s;
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
        string[] a = {"arcane_bolt", "magic_missile", "arcane_blast", "arcane_spray", "straight_slice"};
        spellTypes = a;
        string[] b = {"damage_amp","speed_amp","doubler","splitter","chaos","homing","knockback","piercing"};
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

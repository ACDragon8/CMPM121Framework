using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;


public class SpellBuilder 
{

    public Spell Build(SpellCaster owner, string spellName="bolt")
    {
        return new Spell(owner);
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
        var spelltext = Resources.Load<TextAsset>("spells");
        JToken jo = JToken.Parse(spelltext.text);
        if (jo != null)
        { 
            //I dunno, do some parsing here???
            //Maybe instantiate all the spell objects here?
        }
        else 
        {
            Debug.Log("Missing spells.json");
        }
    }

}

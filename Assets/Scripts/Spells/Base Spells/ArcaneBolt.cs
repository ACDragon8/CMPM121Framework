using Newtonsoft.Json.Linq;
using UnityEngine;

public class ArcaneBolt : Spell
{
    
    public ArcaneBolt(SpellCaster owner) : base(owner)
    {
        this.owner = owner;
    }

    public override void SetProperties(JObject spellAttributes)
    {
        //Read and parse the Json object here
        base.SetProperties(spellAttributes);
    }
}

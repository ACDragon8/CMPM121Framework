using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class Relic
{
    public static JObject RelicAttributes = JObject.Parse(Resources.Load<TextAsset>("relics").text);

    public string name;
    public int sprite;

    public JObject trigger;
    public JObject effect;
    public SpellCaster owner;





    public Relic(SpellCaster owner,string name)
    {
        this.owner = owner;
        this.name = name;
        this.sprite = RelicAttributes[name]["sprite"].ToObject<int>();
        this.trigger = RelicAttributes[name]["trigger"].ToObject<JObject>();
        this.effect = RelicAttributes[name]["effect"].ToObject<JObject>();
    }



    private class Trigger
    {
        string description;
        string type;
        int amount;


    }

    private class Effect
    {
        string description;
        string type;
        int amount;
        int until;
    }
}
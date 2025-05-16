using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class Relic
{
    public static JObject RelicAttributes = JObject.Parse(Resources.Load<TextAsset>("relics").text);

    public string name;
    public int sprite;

    public string trigger;
    public string effect;





    public Relic(string name)
    {
        this.name = name;
        this.sprite = RelicAttributes[name]["sprite"].ToObject<int>();
        this.trigger = RelicAttributes[name]["trigger"].ToString();
        this.effect = RelicAttributes[name]["sprite"].ToString();
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
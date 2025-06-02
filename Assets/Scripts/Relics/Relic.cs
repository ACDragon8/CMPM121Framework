using Newtonsoft.Json.Linq;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Relic
{
    public static JObject RelicAttributes = JObject.Parse(Resources.Load<TextAsset>("relics").text);

    public string name;
    public int sprite;

    public JObject trigger;
    public JObject effect;
    public SpellCaster owner;





    public Relic(string name)
    {
        this.name = name;
        this.sprite = RelicAttributes[name]["sprite"].ToObject<int>();
        this.trigger = RelicAttributes[name]["trigger"].ToObject<JObject>();
        this.effect = RelicAttributes[name]["effect"].ToObject<JObject>();
        //owner = GameManager.Instance.player.GetComponent<PlayerController>().spellcaster;
    }

    public string GetName() { return name; }
    public int GetIcon() { return sprite; }
    public string GetDescription() 
    {
        return trigger["description"] + ", " + effect["description"];
    }
    public virtual void Activate() {
        owner = GameManager.Instance.player.GetComponent<PlayerController>().spellcaster;
    }
    public virtual void Deactivate() { }
    public class Trigger
    {
        public string description;
        string type;
        int amount;


    }

    public class Effect
    {
        public string description;
        string type;
        int amount;
        int until;
    }
}
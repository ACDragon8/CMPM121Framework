using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class Blood : Relic
{


    public Blood() : base("Blood Red Night") { }
    public void onTrigger(Vector3 where, Damage damage, Hittable target)
    {
        var value = ReversePolishCalc.Calculate(this.effect["amount"].ToString().Split());
        GameManager.Instance.player.GetComponent<PlayerController>().hp.Heal(value);
        //Debug.Log("Gain mana");
    }
    public override void Activate() { EventBus.Instance.OnDamage += onTrigger; }
    public override void Deactivate() { EventBus.Instance.OnDamage -= onTrigger; }
}
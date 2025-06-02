using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class GreenGem : Relic
{


    public GreenGem() : base("Green Gem"){ }
    public override void Activate() { 
        base.Activate();
        EventBus.Instance.OnDamage += onTrigger; }
    public override void Deactivate() { EventBus.Instance.OnDamage -= onTrigger; }
    public void onTrigger(Vector3 where, Damage damage, Hittable target)
    {
        if (target != GameManager.Instance.player.GetComponent<PlayerController>().hp)
        {
            return;
        }
        var value = ReversePolishCalc.Calculate(this.effect["amount"].ToString().Split());
        owner.gainMana(value);
        //Debug.Log("Gain mana");
    }
}
using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class GreenGem : Relic
{


    public GreenGem() : base("Green Gem")
    {
        
        EventBus.Instance.OnDamage += onTrigger;
    }
    public void onTrigger(Vector3 where, Damage damage, Hittable target)
    {
        if (target != GameManager.Instance.player.GetComponent<PlayerController>().hp)
        {
            return;
        }
        var value = ReversePolishCalc.Calculate(this.effect["amount"].ToString().Split());
        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.gainMana(value);
        //Debug.Log("Gain mana");
    }
}
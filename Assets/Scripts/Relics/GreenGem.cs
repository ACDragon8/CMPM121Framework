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
        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.gainMana(5);
        //Debug.Log("Gain mana");
    }
}
using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class GoldenMask : Relic
{


    public GoldenMask() : base("Golden Mask")
    {
        EventBus.Instance.OnDamage += onTrigger;
    }

    public void onTrigger(Vector3 where, Damage damage, Hittable target)
    {
        //GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.gainMana(5);
        Debug.Log("Gain spell power for next spell");
    }
}
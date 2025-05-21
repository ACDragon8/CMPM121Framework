using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class GoldenMask : Relic
{


    public GoldenMask(SpellCaster owner) : base(owner, "Golden Mask")
    {
        EventBus.Instance.OnDamage += onTrigger;
        EventBus.Instance.Cast += onReset;
    }

    public void onTrigger(Vector3 where, Damage damage, Hittable target)
    {
        if (target != GameManager.Instance.player.GetComponent<PlayerController>().hp)
        {
            return;
        }
        var value = ReversePolishCalc.Calculate(this.effect["amount"].ToString().Split());
        owner.modifyPower(this.name,value);
        Debug.Log("Gain spell power for next spell");
    }

    public void onReset()
    {
       owner.modifyPower(this.name,0);
    }
}
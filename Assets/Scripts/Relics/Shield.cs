using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class Shield : Relic
{


    public Shield() : base("Brittle Shield")
    {
        EventBus.Instance.OnDamage += onTrigger;
        EventBus.Instance.SwitchSpell += onReset;
    }

    public void onTrigger(Vector3 where, Damage damage, Hittable target)
    {
        if (target != GameManager.Instance.player.GetComponent<PlayerController>().hp)
        {
            return;
        }
        var value = ReversePolishCalc.Calculate(this.effect["amount"].ToString().Split());
        GameManager.Instance.player.GetComponent<PlayerController>().modifySpeed(name, value);
        //Debug.Log("zoom");
    }

    public void onReset()
    {
       GameManager.Instance.player.GetComponent<PlayerController>().modifySpeed(name, 0);
    }
}
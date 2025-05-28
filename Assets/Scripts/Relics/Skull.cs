using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class Skull : Relic
{


    public Skull(SpellCaster owner) : base(owner, "Golden Mask")
    {
        EventBus.Instance.SwitchSpell += onTrigger;
        EventBus.Instance.Cast += onReset;
    }

    public void onTrigger()
    {
        var value = ReversePolishCalc.Calculate(this.effect["amount"].ToString().Split());
        owner.modifyPower(this.name,value);
        Debug.Log("Gain spell power for next spell");
    }

    public void onReset()
    {
       owner.modifyPower(this.name,0);
    }
}
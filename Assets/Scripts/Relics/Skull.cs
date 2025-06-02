using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class Skull : Relic
{


    public Skull() : base("Funny Skull")
    {
    }
    public override void Activate() {
        base.Activate();
        EventBus.Instance.SwitchSpell += onTrigger;
        EventBus.Instance.Cast += onReset;
    }
    public override void Deactivate() {
        EventBus.Instance.SwitchSpell -= onTrigger;
        EventBus.Instance.Cast -= onReset;
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
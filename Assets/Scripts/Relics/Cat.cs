using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class Cat : Relic
{
    public Cat() : base("Cat the Kat")
    {
    }
    public override void Activate() {
        base.Activate();
        EventBus.Instance.Cat += onTrigger;
        EventBus.Instance.Cast += onReset;
    }
    public override void Deactivate() {
        EventBus.Instance.Cat -= onTrigger;
        EventBus.Instance.Cast -= onReset;
    }
    public void onTrigger()
    {
        var rnd = Random.value;
        if (rnd <= 0.01)
        {
            var value = ReversePolishCalc.Calculate(this.effect["amount"].ToString().Split());
            //Debug.Log("MAXWELL!");
            owner.modifyPower(this.name, value);
        }
    }

    public void onReset()
    {
        owner.modifyPower(this.name, 0);
        //Debug.Log("meow");
    }
}
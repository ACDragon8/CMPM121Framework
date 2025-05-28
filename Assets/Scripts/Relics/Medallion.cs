using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;
public class Medallion : Relic
{
    public Medallion (SpellCaster owner) : base(owner, "Blue Medallion")
    {
        EventBus.Instance.OnDeath += onTrigger;
        EventBus.Instance.Cast += onReset;
    }

    public void onTrigger(Vector3 where, Hittable target)
    {
        var value = ReversePolishCalc.Calculate(this.effect["amount"].ToString().Split());
        owner.modifyPower(name,value);
        //Debug.Log("gain mana 25");
    }

    public void onReset()
    {
        owner.modifyPower(name, 0);
    }

}
using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;
public class Medallion : Relic
{
    public Medallion () : base("Blue Medallion")
    {
        EventBus.Instance.OnDeath += onTrigger;
        EventBus.Instance.Cast += onReset;
    }

    public void onTrigger(Vector3 where, Hittable target)
    {
        var value = ReversePolishCalc.Calculate(this.effect["amount"].ToString().Split());
        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.modifyPower(name,value);
        //Debug.Log("gain mana 25");
    }

    public void onReset()
    {
        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.modifyPower(name, 0);
    }

}
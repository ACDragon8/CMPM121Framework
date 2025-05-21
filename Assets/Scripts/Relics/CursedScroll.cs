using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;
public class CursedScroll : Relic
{
    public CursedScroll(SpellCaster owner) : base(owner, "Cursed Scroll")
    {
        EventBus.Instance.OnDeath += onTrigger;
    }

    public void onTrigger(Vector3 where, Hittable target)
    {
        var value = ReversePolishCalc.Calculate(this.effect["amount"].ToString().Split());
        owner.gainMana(value);
        //Debug.Log("gain mana 25");
    }

}
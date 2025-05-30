using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;
public class CursedScroll : Relic
{
    public CursedScroll() : base("Cursed Scroll")
    {
        EventBus.Instance.OnDeath += onTrigger;
    }

    public void onTrigger(Vector3 where, Hittable target)
    {
        var value = ReversePolishCalc.Calculate(this.effect["amount"].ToString().Split());
        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.gainMana(value);
        //Debug.Log("gain mana 25");
    }

}
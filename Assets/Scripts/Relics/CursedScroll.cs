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
        owner.gainMana(25);
        //Debug.Log("gain mana 25");
    }

}
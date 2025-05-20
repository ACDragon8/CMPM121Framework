using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class GreenGem : Relic
{


    public GreenGem(SpellCaster owner) : base(owner, "Green Gem")
    {
        EventBus.Instance.OnDamage += onTrigger;
    }

    public void onTrigger(Vector3 where, Damage damage, Hittable target)
    {
       owner.gainMana(25);
        //Debug.Log("Gain mana");
    }
}
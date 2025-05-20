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
        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.gainMana(25);
        Debug.Log("gain mana 25");
    }

}
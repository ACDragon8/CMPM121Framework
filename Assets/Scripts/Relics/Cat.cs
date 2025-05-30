using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class Cat : Relic
{
    public Cat() : base("Cat the Kat")
    {
        EventBus.Instance.Cat += onTrigger;
        EventBus.Instance.Cast += onReset;
    }

    public void onTrigger()
    {
        var rnd = Random.value;
        if (rnd <= 0.01)
        {
            var value = ReversePolishCalc.Calculate(this.effect["amount"].ToString().Split());
            //Debug.Log("MAXWELL!");
            GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.modifyPower(this.name, value);
        }
    }

    public void onReset()
    {
        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.modifyPower(this.name, 0);
        //Debug.Log("meow");
    }
}
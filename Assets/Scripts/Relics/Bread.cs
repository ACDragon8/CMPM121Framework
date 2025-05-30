using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class Bread : Relic
{
    private float endTime;
    private bool active;
    public Bread() : base("Bread")
    {
        active = false;
        endTime = Time.time;
        EventBus.Instance.OnDamage += onTrigger;
        EventBus.Instance.Move += onReset;
    }

    public void onTrigger(Vector3 where, Damage damage, Hittable target)
    {

        var value = ReversePolishCalc.Calculate(this.effect["amount"].ToString().Split());
        GameManager.Instance.player.GetComponent<PlayerController>().modifySpeed(name, value);
        active = true;
        var duration = ReversePolishCalc.Calculate(this.effect["duration"].ToString().Split());
        endTime = Time.time + duration;
        Debug.Log("fast");

    }

    void onReset()
    {
        if (active)
        {
            if (Time.time >= endTime)
            {
                Debug.Log("slow");
                GameManager.Instance.player.GetComponent<PlayerController>().modifySpeed(name, 0);
                active = false;
            }
        }


    }
}
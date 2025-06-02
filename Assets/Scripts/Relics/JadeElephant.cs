using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class JadeElephant : Relic
{

    public JadeElephant() : base("Jade Elephant")
    {
    }
    public override void Activate() {
        //on stand still
        base.Activate();
        EventBus.Instance.Idle += onTrigger;
        EventBus.Instance.Move += onReset;
    }
    public override void Deactivate() {
        EventBus.Instance.Idle -= onTrigger;
        EventBus.Instance.Move -= onReset;
    }
    public void onTrigger()
    {
        var s = effect["amount"].ToString().Split(' ');
        int index = 0;
        foreach (string token in s) 
        {
            if (token == "wave")
            {
                s[index] = StatsManager.Instance.waveNum.ToString();
            }
            else if (token == "power") 
            {
                //We get the power value from the owner (spellcaster class passed in)
                s[index] = owner.power.ToString();
            }
            index++;
        }
        int value = ReversePolishCalc.Calculate(s);
        owner.modifyPower(this.name, value);
    }
    public void onReset()
    {
        owner.modifyPower(this.name,0);
    }
}
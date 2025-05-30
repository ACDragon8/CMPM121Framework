using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class JadeElephant : Relic
{

    public JadeElephant() : base("Jade Elephant")
    {
        //on stand still
        EventBus.Instance.Idle += onTrigger;
        EventBus.Instance.Move += onReset;
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
                s[index] = GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.power.ToString();
            }
            index++;
        }
        int value = ReversePolishCalc.Calculate(s);
        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.modifyPower(this.name, value);
    }
    public void onReset()
    {
        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.modifyPower(this.name,0);
    }
}
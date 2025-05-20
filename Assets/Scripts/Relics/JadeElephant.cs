using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class JadeElephant : Relic
{

    private ReversePolishCalc RPN;

    public JadeElephant(SpellCaster owner) : base(owner, "Jade Elephant")
    {
        this.RPN = new ReversePolishCalc();
        //on stand still
        EventBus.Instance.Idle += onTrigger;
        EventBus.Instance.Move += onReset;
    }

    public void onTrigger()
    {
        var s = this.effect["amount"].ToString().Split(' ');
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
                s[index] = this.owner.power.ToString();
            }
            index++;
        }
        var value = ReversePolishCalc.Calculate(s);
        owner.modifyPower(this.name, value);
    }

    public void onReset()
    {
GameManager.Instance.player.GetComponent<PlayerController>(). spellcaster.modifyPower(this.name,0);

    }
}
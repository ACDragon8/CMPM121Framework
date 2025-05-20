using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class JadeElephant : Relic
{


    public JadeElephant() : base("JadeElephant")
    {
        //on stand still
        EventBus.Instance.Idle += onTrigger;
    }

    public void onTrigger()
    {
        //modify to gain spell power
        Debug.Log("gain power");
        //also implement losing spell power
    }
}
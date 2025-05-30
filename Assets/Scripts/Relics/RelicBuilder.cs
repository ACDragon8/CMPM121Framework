using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class RelicBuilder : MonoBehaviour
{
    public ArrayList avaliable_relics;
    private void Start()
    {
        avaliable_relics = new ArrayList();
        //This solution below is terrible but I couldn't get monkey brain to work
        //And again, curse you dragon for being a coward
        avaliable_relics.Add(new Blood());
        avaliable_relics.Add(new Bread());
        avaliable_relics.Add(new Cat());
        avaliable_relics.Add(new CursedScroll());
        avaliable_relics.Add(new GoldenMask());
        avaliable_relics.Add(new GreenGem());
        avaliable_relics.Add(new Medallion());
        avaliable_relics.Add(new Shield());
        avaliable_relics.Add(new Skull());
    }
    
}
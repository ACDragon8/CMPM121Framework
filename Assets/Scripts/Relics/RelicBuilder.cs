using System.Collections;
using UnityEngine;

public class RelicBuilder
{
    public ArrayList avaliable_relics;
    public SpellCaster player;
    public RelicBuilder(SpellCaster spellcaster)
    {
        //EventBus.Instance.OnPlayerInitialized += SetPlayer;
        avaliable_relics = new ArrayList();
        player = spellcaster;
        //player = GameManager.Instance.player.GetComponent<PlayerController>().spellcaster;
        Debug.Log("Setting player and adding relics to the list");
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
    public int GetNumAvaliableRelics() {
        //Debug.Log("Giving the list length which is " + avaliable_relics.Count);
        Debug.Log(avaliable_relics.Count);
        return avaliable_relics.Count;
    }
    public int ChooseRandomRelic() {
        return Random.Range(0, avaliable_relics.Count);
    }
    public Relic GetRelic(int relic_index) {
        if (relic_index < 0 || relic_index >= avaliable_relics.Count) {
            Debug.Log("Requested Relic index out of range");
            return null;
        }
        return (Relic) avaliable_relics[relic_index];
    }
    public void RemoveRelic(int relic_index) {
        //Activate the relic
        avaliable_relics.RemoveAt(relic_index);
    }
}
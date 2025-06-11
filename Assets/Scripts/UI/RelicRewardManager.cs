using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;

public class RelicRewardManager : MonoBehaviour 
{
    // It is up to the RelicRewardManager to pick relics using the Relic builder and assign them to the UI elements 
    public GameObject[] relic_rewards;
    public GameObject craftingManager;
    public Action nextRewardDisplay;
    private RelicBuilder rb;
    public int relic_amount = 3;
    public int spacing = 200;
    public void Instantiate()
    {
        GameManager.Instance.LevelStart += GenerateRelics;
        EventBus.Instance.OnRelicPickup += EquipRelic;
    }
    public void DisplayRelicOptions() {
        if (StatsManager.Instance.waveNum % 3 == 0 || StatsManager.Instance.levelName == "Debug")
        {
            this.gameObject.SetActive(true);
            //int display_amount = relic_amount > rb.GetNumAvaliableRelics() ? rb.GetNumAvaliableRelics() : relic_amount;
            if (relic_amount < rb.GetNumAvaliableRelics())
            {
                Debug.Log("a");
                int[] already_seen = new int[relic_amount];
                int displays_set = 0;
                while (displays_set < 3) 
                {
                    bool seen = false;
                    int relic_index = rb.ChooseRandomRelic();
                    for (int i = 0; i < displays_set; i++) {
                        if (already_seen[i] == relic_index) {
                            seen = true;
                            break;
                        }
                    }
                    if (seen)
                    {
                        continue;
                    }
                    else
                    {
                        already_seen[displays_set] = relic_index;
                    }
                    Relic r = rb.GetRelic(relic_index);
                    relic_rewards[displays_set].GetComponent<RelicRewardDisplay>().SetRelic(r);
                    relic_rewards[displays_set].GetComponent<RelicRewardDisplay>().relic_index = relic_index;
                    relic_rewards[displays_set].transform.localPosition = new Vector3((spacing * displays_set) - 200, 0);
                    relic_rewards[displays_set].SetActive(true);
                    already_seen[displays_set] = relic_index;
                    displays_set++;
                }
            }
            else 
            {
                Debug.Log("b");
                //TODO figure out why its lying to me
                if (rb.GetNumAvaliableRelics() == 0)
                {
                    Debug.Log("No relics left for rewards");
                    HideRelicRewards();
                }
                for (int i = 0; i < rb.GetNumAvaliableRelics(); i++) 
                {
                    Relic r = rb.GetRelic(i);
                    relic_rewards[i].GetComponent<RelicRewardDisplay>().SetRelic(r);
                    relic_rewards[i].GetComponent<RelicRewardDisplay>().relic_index = i;
                    relic_rewards[i].SetActive(true);
                    //TODO make the spacing nicer if less than 3 relic rewards avaliable    
                    relic_rewards[i].transform.localPosition = new Vector3((spacing * i) - 200, 0);
                }
            }
        }
        else 
        {
            Debug.Log("It's not relic time yet ;-;");
            HideRelicRewards();
        }
    }
    public void HideRelicRewards() {
        this.gameObject.SetActive(false);
        foreach (var display in relic_rewards)
        {
            display.SetActive(false);
        }
        nextRewardDisplay?.Invoke();
    }
    public void EquipRelic(Relic r, int index) {
        rb.RemoveRelic(index);
        GameManager.Instance.player.GetComponent<PlayerController>().relics.Add(r);
        r.Activate();
        HideRelicRewards();
    }
    public void GenerateRelics()
    {
        SpellCaster spellcaster = GameManager.Instance.player.GetComponent<PlayerController>().spellcaster;
        rb = new RelicBuilder(spellcaster);
        //such spaghetti    
        craftingManager.GetComponent<CraftingManager>().relicBuilder = rb;
    }
}

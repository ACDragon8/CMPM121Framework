using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView;

public class RelicRewardDisplay : MonoBehaviour
{
    // It is up to the RelicRewardManager to pick relics using the Relic builder and assign them to the UI elements 
    public GameObject icon;
    public TextMeshProUGUI relic_name;
    public TextMeshProUGUI relic_description;
    public Relic relic;
    public int relic_index;
    public void SetRelic(Relic r)
    {
        relic = r;
        GameManager.Instance.relicIconManager.PlaceSprite(r.GetIcon(), icon.GetComponent<Image>());
        relic_name.text = r.GetName();
        relic_description.text = r.GetDescription();
    }
    public void AcceptRelic() {
        EventBus.Instance.OnRelicPickupEffect(relic, relic_index);
    }
}

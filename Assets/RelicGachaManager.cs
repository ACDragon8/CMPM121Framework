using UnityEngine;

public class RelicGachaManager : MonoBehaviour
{
    public GameObject GambleButton;
    public GameObject ReturnButton;
    public GameObject RelicDisplay;
    public GameObject CraftingManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventBus.Instance.OnRelicCrafted += DisplayRelicResult;
        EventBus.Instance.OnRelicPickup += RelicPickup;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HideRelicGacha() {
        this.gameObject.SetActive(false);
    }
    public void Gamble() {
        CraftingManager.GetComponent<CraftingManager>().GachaRelic();
        DisplayRelicResult(CraftingManager.GetComponent<CraftingManager>().relic);
    }
    public void DisplayRelicResult(Relic r) {
        RelicDisplay.SetActive(true);
        RelicDisplay.GetComponent<RelicRewardDisplay>().SetRelic(r);
    }
    public void RelicPickup(Relic r, int index) { RelicDisplay.SetActive(false); }
}

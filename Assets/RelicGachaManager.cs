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
        DisplayRelicResult();
    }
    public void DisplayRelicResult()
    {
        
        if (CraftingManager.GetComponent<CraftingManager>().relic == null)
        {
            return;
        }
        RelicDisplay.GetComponent<RelicRewardDisplay>().SetRelic(CraftingManager.GetComponent<CraftingManager>().relic);
        RelicDisplay.GetComponent<RelicRewardDisplay>().SetIndex(CraftingManager.GetComponent<CraftingManager>().index);
        RelicDisplay.SetActive(true);
    }
    public void RelicPickup()
    {
        RelicDisplay.GetComponent<RelicRewardDisplay>().AcceptRelic();
        EventBus.Instance.OnRelicCraftedEffect();
        RelicDisplay.SetActive(false);
    }
}

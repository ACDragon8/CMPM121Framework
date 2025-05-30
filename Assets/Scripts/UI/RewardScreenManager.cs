using UnityEngine;

public class RewardScreenManager : MonoBehaviour
{
    public GameObject rewardUI;
    //These are children to the reward UI
    public GameObject SpellRewardDisplay;
    public GameObject RelicRewardDisplay;
    public GameObject NextWaveButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpellRewardManager spell_reward_ui = SpellRewardDisplay.GetComponent<SpellRewardManager>();
        spell_reward_ui.nextRewardDisplay += ShowNextWaveButton;  //TODO for now just but the next button for reward screen, in the future, change
        //it to be the relic reward screen manager
        GameManager.Instance.OnWaveEnd += ShowRewards;
    }



    public void ShowRewards()
    {
        rewardUI.SetActive(true);
        SpellRewardManager spell_reward_ui = SpellRewardDisplay.GetComponent<SpellRewardManager>();
        spell_reward_ui.DisplaySpellReward();
        //Each reward manager acts like a node in a linked list that points to the next reward that supposed to be shown        
    }
    public void HideRewardScreen() {
        NextWaveButton.SetActive(false);
        rewardUI.SetActive(false);
        GameManager.Instance.OnRewardSelectionFinishedEffects();
    }
    public void ShowNextWaveButton() { NextWaveButton.SetActive(true); }
}

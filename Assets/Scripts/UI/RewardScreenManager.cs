using UnityEngine;

public class RewardScreenManager : MonoBehaviour
{
    public GameObject rewardUI;
    //These are children to the reward UI
    public GameObject SpellRewardDisplay;
    public GameObject RelicRewardManager;
    public GameObject NextWaveButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpellRewardManager spell_reward_ui = SpellRewardDisplay.GetComponent<SpellRewardManager>();
        RelicRewardManager relic_reward_ui = RelicRewardManager.GetComponent<RelicRewardManager>();
        spell_reward_ui.nextRewardDisplay += relic_reward_ui.DisplayRelicOptions;
        relic_reward_ui.nextRewardDisplay += ShowNextWaveButton;
        relic_reward_ui.Instantiate();
        GameManager.Instance.OnWaveEnd += ShowRewards;
    }



    public void ShowRewards()
    {
        rewardUI.SetActive(true);
        SpellRewardManager spell_reward_ui = SpellRewardDisplay.GetComponent<SpellRewardManager>();
        spell_reward_ui.DisplaySpellReward();
        //Each reward manager acts like a node in a linked list that points to the next reward that supposed to be shown
        RelicRewardManager.SetActive(false);
    }
    public void HideRewardScreen() {
        NextWaveButton.SetActive(false);
        rewardUI.SetActive(false);
        GameManager.Instance.OnRewardSelectionFinishedEffects();
    }
    public void ShowNextWaveButton() { NextWaveButton.SetActive(true); }
}

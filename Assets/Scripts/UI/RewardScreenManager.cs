using UnityEngine;

public class RewardScreenManager : MonoBehaviour
{
    public GameObject rewardUI;
    public GameObject SpellSelector;
    public const int spacing = 50;
    public bool gamelock;

    public GameObject[] spellButtons;

    
    public const int rewardNumber = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spellButtons = new GameObject[rewardNumber];
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND)
        {
            if(!gamelock) {
            gamelock = true;
            rewardUI.SetActive(true);
            DestroyButtons();
            
            for(int i = 0; i < rewardNumber;i++) {
                spellButtons[i] = Instantiate(SpellSelector, rewardUI.transform);
                spellButtons[i].transform.localPosition = new Vector3(0, spacing*i - 50);
                spellButtons[i].GetComponent<SpellRewardManager>().rsw = this;
                spellButtons[i].GetComponent<SpellRewardManager>().player =  GameManager.Instance.player.GetComponent<PlayerController>().spellcaster;
                spellButtons[i].GetComponent<SpellRewardManager>().pickSpell();
            }
            }
        }
        else
        {
            if(gamelock) {
                gamelock = false;
            }
            rewardUI.SetActive(false);
        }
    }

    public void DestroyButtons() {
        for(int i = 0; i < rewardNumber;i++) {s
            if(spellButtons[i] != null) {
                Destroy(spellButtons[i]);
                spellButtons[i] = null;
            }
        }
    }
}

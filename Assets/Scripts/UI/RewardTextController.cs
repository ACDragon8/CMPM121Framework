using TMPro;
using UnityEngine;

public class RewardTextController : MonoBehaviour
{
    TextMeshProUGUI tmp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND)
        {
            tmp.text = "Waves survived: " + StatsManager.Instance.GetWaveNum() + "\n";
        }
    }
}

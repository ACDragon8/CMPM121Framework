using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView;

public class MoneyDisplay : MonoBehaviour
{
    public GameObject CraftingManager;
    public TextMeshProUGUI money;
    public int coins;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventBus.Instance.ChangeMoney += UpdateCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateCount(int amt)
    {
        coins = CraftingManager.GetComponent<CraftingManager>().materials["coin"];
        money.text = "Coins:" + coins;

    }
}

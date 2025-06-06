using UnityEngine;
using System.Collections;


public class CraftingManager : MonoBehaviour
{

    public GameObject craftUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenCraftMenu()
    {
        craftUI.SetActive(true);

    }

    public void CloseCraftMenu()
    {
        craftUI.SetActive(false);
    }


}

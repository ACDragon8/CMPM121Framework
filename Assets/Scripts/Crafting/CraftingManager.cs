using UnityEngine;
using System.Collections;


public class CraftingManager : MonoBehaviour
{

    public GameObject craftUI;
    public int materials;
    private bool toggle;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        toggle = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ToggleCraftMenu()
    {
        if (toggle) {
            toggle = false;
        }
        else {
            toggle = true;
        }
        craftUI.SetActive(toggle);
    }

    public void OpenCraftMenu()
    {
        craftUI.SetActive(true);

    }

    public void CloseCraftMenu()
    {
        craftUI.SetActive(false);
    }

    public bool checkCraftable()
    {
        return false;
    }


}

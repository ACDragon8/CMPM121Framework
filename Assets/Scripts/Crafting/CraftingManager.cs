using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Unity.VisualScripting;


public class CraftingManager : MonoBehaviour
{

    public GameObject craftUI;
    public Dictionary<string, int> materials;
    private bool toggle;

    public static string[] materialList = { "coin" };

    JObject spellList;
    JObject relicList;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        toggle = false;

        materials = new Dictionary<string, int>();
        foreach (string item in materialList)
        {
            materials.Add(item, 0);
        }

        materials["coin"] += 11;


        //load spell list
        var spelltext = Resources.Load<TextAsset>("spells");
        JObject jo = JObject.Parse(spelltext.text);
        if (jo != null)
        {
            spellList = jo;
        }
        else
        {
            Debug.Log("Missing spells.json");
        }
        //load relic list
        var relictext = Resources.Load<TextAsset>("relics");
        jo = JObject.Parse(relictext.text);
        if (jo != null)
        {
            relicList = jo;
        }
        else
        {
            Debug.Log("Missing relics.json");
        }

        //



    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ToggleCraftMenu()
    {
        if (toggle)
        {
            toggle = false;
        }
        else
        {
            toggle = true;
        }
        craftUI.SetActive(toggle);
        //temp testing stuff-------------------------------------------DELETE-----------
        Debug.Log(materials["coin"]);
        Craft("magic_issile");

    }

    public void OpenCraftMenu()
    {
        toggle = true;
        craftUI.SetActive(true);

    }

    public void CloseCraftMenu()
    {
        toggle = false;
        craftUI.SetActive(false);
    }

    public Dictionary<string, int> GetRecipe(string name)
    {
        var recipe = spellList[name]["recipe"].ToObject<Dictionary<string, int>>();
        return recipe;
    }

    public bool CheckCraftable(string name)
    {
        Dictionary<string, int> recipe;
        //load recipe from json
            recipe = spellList[name]["recipe"].ToObject<Dictionary<string, int>>();
        foreach (var (item, cost) in recipe)
        {
            if (materials[item] < cost)
            {
                return false;
            }
        }
        return true;
    }


    public void Craft(string name)
    {
        //check if craftble, if not then do nothing, otherwise continue
        if (!CheckCraftable(name))
        {
            return;
        }
        //add the spell if possible, otherwise do nothing
        bool res = GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.addSpell(name);
        if (!res)
        {
            return;
        }

        //pay the price
        Dictionary<string, int> recipe = spellList[name]["recipe"].ToObject<Dictionary<string, int>>();
        foreach (var (item, cost) in recipe)
        {
            materials[item] -= cost;
        }
        return;

    }



}

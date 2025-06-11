using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Unity.VisualScripting;


public class CraftingManager : MonoBehaviour
{
    public GameObject menuBackground;
    public GameObject craftUI;
    public GameObject gachaMenu;
    public Dictionary<string, int> materials;
    private bool toggle;
    public Relic relic;
    public int index;

    public static string[] materialList = { "coin" };

    public RelicBuilder relicBuilder; //SPAGHETTI CODE NOTICE: THIS IS SET IN RelicRewardManger GenerateRelics

    JObject spellList;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventBus.Instance.OnDeath += DropMaterials;
        toggle = false;
        relic = null;
        index = -1;

        materials = new Dictionary<string, int>();
        foreach (string item in materialList)
        {
            materials.Add(item, 0);
        }

        //DEBUG
        materials["coin"] += 10;


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

        EventBus.Instance.OnSpellCasterInitialized += InstantiateSpellCraftMenu;
        EventBus.Instance.OnSpellCrafted += CraftWithSpell;
        EventBus.Instance.OnRelicCrafted += ResetRelic;
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

    }
    public void InstantiateSpellCraftMenu(SpellCaster sc)
    {
        craftUI.GetComponent<BaseSpellListUI>().Instantiate();
        craftUI.GetComponent<BaseSpellListUI>().GenerateBaseSpells(sc, 2);
    }
    public void OpenCraftMenu()
    {
        toggle = true;
        craftUI.SetActive(true);
        menuBackground.SetActive(true);
    }

    public void CloseCraftMenu()
    {
        toggle = false;
        craftUI.SetActive(false);
        menuBackground.SetActive(false);
    }

    public Dictionary<string, int> GetRecipe(string name)
    {
        var recipe = spellList[name]["recipe"].ToObject<Dictionary<string, int>>();
        return recipe;
    }

    public string GetName(string name)
    {
        return spellList[name]["name"].ToString();
    }

    public string jsonify(string name)
    {
        return name.ToLower().Replace(" ", "_");
    }

    public string GetDescription(string name)
    {
        return spellList[name]["description"].ToString();
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
            Debug.Log("Error, no avaliable spell slots to equip");
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

    public void CraftWithSpell(Spell sp)
    {
        Craft(jsonify(sp.GetName()));
    }
    public void ShowGachaMenu() { gachaMenu.SetActive(true); menuBackground.SetActive(true); }
    public void HideGachaMenu() { gachaMenu.SetActive(false); menuBackground.SetActive(false); }
    public void GachaRelic()
    {
        if (relicBuilder != null)
        {
            if (materials["coin"] < 10)
            {
                return;
            }
            materials["coin"] -= 10;
            index = relicBuilder.ChooseRandomRelic();
            relic = relicBuilder.GetRelic(index);
        }


    }
    public void EquipRelic(Relic r, int index)
    {
        if (relicBuilder != null)
        {
            relicBuilder.RemoveRelic(index);
            GameManager.Instance.player.GetComponent<PlayerController>().relics.Add(r);
            r.Activate();
            relic = null;
        }

    }

    void DropMaterials(Vector3 where, Hittable hp)
    {
        foreach (var item in materialList)
        {
            materials[item] += Random.Range(0, 3);
        }
    }

    void ResetRelic()
    {
        relic = null;
        index = -1;
    }



}

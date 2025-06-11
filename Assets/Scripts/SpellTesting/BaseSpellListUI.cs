using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;



public class BaseSpellListUI : MonoBehaviour
{
    public GameObject Prev_button;
    public GameObject Next_button;
    public GameObject Return_button;
    public TextMeshProUGUI page_num;
    private SpellCaster sc;
    private SpellBuilder spellBuilder;
    [SerializeField] private List<GameObject> BaseSpellList;
    [SerializeField] private int current_page;
    [SerializeField] private int total_pages;
    [SerializeField] private int spells_per_page = 6;
    //This is for instantiating stuff
    public GameObject SpellRewardUI;
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateBaseSpells(SpellCaster spellcaster) {
        sc = spellcaster;
        //This is the spacing for Spell Tester
        //Subtract 100 from Y value every iteration unless its the 4th
        //On the 4th one, reset Y back to 140
        //X is either -420 or 60
        int y_offset = 0;
        total_pages = 0;
        foreach (string base_spell in spellBuilder.spellTypes) {
            if (y_offset % spells_per_page == 0) {
                total_pages += 1;
            }
            int half_page = (spells_per_page / 2);
            GameObject spell_ui = Instantiate(SpellRewardUI, this.transform);
            int x = y_offset < half_page ? -420 : 60;
            int y = 140 - 100 * (y_offset%half_page);
            spell_ui.transform.position = this.transform.position + new Vector3 (x, y, 0);
            Spell speel = spellBuilder.Build(spellcaster, base_spell);
            spell_ui.GetComponent<SpellRewardManager>().ShowSpecificSpell(speel);
            BaseSpellList.Add(spell_ui);
            y_offset++;
        }
        ShowFirstPage();
    }
    public void GenerateBaseSpells(SpellCaster spellcaster, int Spells_per_page)
    {
        sc = spellcaster;
        //This is the spacing for the crafting menu
        //It won't have double columns like spell tester
        int y_offset = 0;
        total_pages = 0;
        spells_per_page = Spells_per_page;
        foreach (string base_spell in spellBuilder.spellTypes)
        {
            if (y_offset % spells_per_page == 0)
            {
                total_pages += 1;
            }
            GameObject spell_ui = Instantiate(SpellRewardUI, this.transform);
            int x = -170;
            int y = 80 - 100 * (y_offset % spells_per_page);
            spell_ui.transform.position = this.transform.position + new Vector3(x, y, 0);
            Spell speel = spellBuilder.Build(spellcaster, base_spell);
            spell_ui.GetComponent<SpellRewardManager>().ShowSpecificSpell(speel);
            spell_ui.SetActive(false);
            BaseSpellList.Add(spell_ui);
            y_offset++;
        }
        ShowFirstPage();
    }
    public void Instantiate() {
        spellBuilder = new SpellBuilder();
        current_page = 1;
    }
    public void SetPageNum() {
        page_num.text = current_page.ToString() + "/" + total_pages.ToString();
    }
    public void ShowFirstPage() {
        for (int i = 0; i < spells_per_page; i++) {
            BaseSpellList[i].SetActive(true);    
        }
        SetPageNum();
    }
    public void ReturnToCraftingMenu() {
        this.gameObject.SetActive(false);
        TrainingRoomEventbus.Instance.onSpellListClosed();
    }

    public void NextPage() {
        //Debug.Log("Next page called!");
        if (current_page == total_pages) return;
        current_page++;
        int curr_spell_index = spells_per_page * (current_page - 1);
        int prev_spell_index = curr_spell_index - spells_per_page;
        for (int i = 0; i < spells_per_page; i++) {
            if (prev_spell_index >= 0) {
                BaseSpellList[prev_spell_index].SetActive(false);
            }
            if (curr_spell_index == BaseSpellList.Count) break;
            BaseSpellList[curr_spell_index].SetActive(true);
            curr_spell_index++;
            prev_spell_index++;
        }
        SetPageNum();
    }
    public void PrevPage()
    {
        //Debug.Log("Previous page called!");
        if (current_page == 1) { return; }
        current_page--;
        int curr_spell_index = spells_per_page * (current_page - 1);
        int prev_spell_index = curr_spell_index + spells_per_page;
        for (int i = 0; i < spells_per_page; i++)
        {
            if (prev_spell_index < BaseSpellList.Count)
            {
                BaseSpellList[prev_spell_index].SetActive(false);
            }
            BaseSpellList[curr_spell_index].SetActive(true);
            curr_spell_index++;
            prev_spell_index++;
        }
        SetPageNum();
    }
}



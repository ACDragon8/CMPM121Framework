using UnityEngine;
using System;
using NUnit.Framework;
using System.Collections.Generic;


//test

public class SpellBuildingManager : MonoBehaviour
{
    public GameObject SpellUIContainer;
    public GameObject SpellBuildingMenu;

    //These two below are UIs that open up other menus
    public GameObject BaseSpellDisplayListUI;
    //public GameObject ModifierDisplayListUI;

    //These two show what is currently chosen
    public GameObject SelectedBaseSpellDisplay;
    public GameObject SelectedModifiersUI;

    public GameObject CraftMenu;

    public GameObject player;
    private SpellCaster spcst;
    [SerializeField] private List<string> modifiers;
    [SerializeField] private Spell base_spell;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TrainingRoomEventbus.Instance.OpenMenu += OpenMenu;
        TrainingRoomEventbus.Instance.SpellListClosed += ShowCraftMenu;
        TrainingRoomEventbus.Instance.SelectBaseSpell += SetBaseSpell;
        spellcasterSYNC();
    }

    public void HideMenu() {
        SpellBuildingMenu.SetActive(false);
        SpellUIContainer.SetActive(true);
        TrainingRoomEventbus.Instance.state = TrainingRoomEventbus.RoomState.COMBAT;
        TrainingRoomEventbus.Instance.OnCloseMenu();
        ClearSpellData();
    }
    public void OpenMenu() {
        if (TrainingRoomEventbus.Instance.state == TrainingRoomEventbus.RoomState.MENU)
        {
            return;
        }
        TrainingRoomEventbus.Instance.state = TrainingRoomEventbus.RoomState.MENU;
        SpellBuildingMenu.SetActive(true);
        SpellUIContainer.SetActive(false);
        //CraftMenu.SetActive(true);
        SelectedBaseSpellDisplay.GetComponent<BaseSpellDisplay>().SetEmpty();
    }

    public void SetBaseSpell(Spell selected_spell) {
        base_spell = selected_spell;
    }
    public void CraftSpell() {
        if (base_spell != null)
        {
            //If there are modifiers, add it to the base spell
            spcst.RemoveAllSpells();
            spcst.addSpell(base_spell);
            ClearSpellData();
            HideMenu();
        }
        else {
            Debug.Log("No selected spell to craft");
        }
    }
    public void ClearSpellData() {
        base_spell = null;
        //Clear out modifier list
    }

    public void spellcasterSYNC() {
        player.GetComponent<SpellUser>().SetStats();
        spcst = player.GetComponent<SpellUser>().spellcaster;
        BaseSpellDisplayListUI.GetComponent<BaseSpellListUI>().Instantiate();
        BaseSpellDisplayListUI.GetComponent<BaseSpellListUI>().GenerateBaseSpells(spcst);
    }

    public void ShowSpellOptions() {
        BaseSpellDisplayListUI.SetActive(true);
        CraftMenu.SetActive(false);
    }
    public void ShowCraftMenu() {
        CraftMenu.SetActive(true);
    }

    public void ShowModifiers() {
        CraftMenu.SetActive(false);
        //Set the modifier menu to true
    }
}



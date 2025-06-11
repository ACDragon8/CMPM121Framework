using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;



public class BaseSpellDisplay : MonoBehaviour
{
    //This is for UI stuff
    public TextMeshProUGUI mana_cost;
    public TextMeshProUGUI damage;
    public GameObject icon;
    public TextMeshProUGUI spell_description_content;
    public TextMeshProUGUI spell_name;
    public TextMeshProUGUI empty;
    public Spell spell;
    void Start()
    {
        TrainingRoomEventbus.Instance.SelectBaseSpell += SetSpell;
        SetEmpty();
    }

    public void SetEmpty() {
        Debug.Log("Setting the selected base spell to empty");
        mana_cost.gameObject.SetActive(false);
        damage.gameObject.SetActive(false);
        spell_name.text = "No Spell Selected";
        spell_description_content.text = "Spell Description unavaliable";
        icon.GetComponent<Image>().sprite = null;
        empty.gameObject.SetActive(true);
    }
    public void ShowIcons() {
        mana_cost.gameObject.SetActive(true);
        damage.gameObject.SetActive(true);
        empty.gameObject.SetActive(false);
    }
    public void SetSpell(Spell selected_spell)
    {
        spell = selected_spell;
        ShowIcons();
        GameManager.Instance.spellIconManager.PlaceSprite(spell.GetIcon(), icon.GetComponent<Image>());
        mana_cost.text = spell.GetManaCost().ToString();
        damage.text = spell.GetDamage().ToString();
        spell_name.text = spell.GetName();
        spell_description_content.text = spell.GetDescription();
    }

}



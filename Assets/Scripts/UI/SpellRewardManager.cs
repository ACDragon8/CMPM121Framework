using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;

public class SpellRewardManager : MonoBehaviour 
{
    public GameObject icon;
    public TextMeshProUGUI manacost;
    public TextMeshProUGUI damage;
    public Spell spell;
    public TextMeshProUGUI spell_description_content;
    public TextMeshProUGUI spell_name;
    public Action nextRewardDisplay;
    public void SetSpell(Spell spell)
    {
        this.spell = spell;
        GameManager.Instance.spellIconManager.PlaceSprite(spell.GetIcon(), icon.GetComponent<Image>());
        manacost.text = spell.GetManaCost().ToString();
        damage.text = spell.GetDamage().ToString();
        spell_name.text = spell.GetName();
        spell_description_content.text = spell.GetDescription();
    }
    public void DisplaySpellReward() {
        SpellBuilder sb = new ();
        Spell reward_spell = sb.RandomBuild(GameManager.Instance.player.GetComponent<PlayerController>().spellcaster);
        SetSpell(reward_spell);
        this.gameObject.SetActive(true);
    }
    public void AcceptSpell() {
        var res = GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.addSpell(spell);
        if (res)
        {
            HideSpellReward();
        }
    }
    public void HideSpellReward() {
        this.gameObject.SetActive(false);
        nextRewardDisplay?.Invoke();
    }

    //These functions below are only used for the spell testing scene
    public TextMeshProUGUI selected_confirmation;
    public GameObject select_button;
    public void ShowSpecificSpell(Spell spell) {
        SetSpell(spell);
        this.gameObject.SetActive(true);
    }
    public void BroadcastSelectedSpell() {
        //Have this broadcast what base spell was selected
        TrainingRoomEventbus.Instance.onSelectBaseSpell(spell);
        EventBus.Instance.OnSpellCraftedEffect(spell);
    }
    private void Start()
    {
        TrainingRoomEventbus.Instance.SelectBaseSpell += SelectedConfirmation;
        TrainingRoomEventbus.Instance.SpellCrafted += ShowSelectButton;
    }
    public void SelectedConfirmation(Spell picked_spell)
    {
        if (picked_spell == spell)
        {
            select_button.SetActive(false);
            //selected_confirmation.gameObject.SetActive(true);
        }
        else {
            ShowSelectButton();
        }
    }
    public void ShowSelectButton() {
        select_button.SetActive(true);
        selected_confirmation.gameObject.SetActive(false);
    }
}

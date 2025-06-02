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
        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.addSpell(spell);
        HideSpellReward();
    }
    public void HideSpellReward() {
        this.gameObject.SetActive(false);
        nextRewardDisplay?.Invoke();
    }
}

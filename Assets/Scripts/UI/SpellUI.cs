using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellUI : MonoBehaviour
{
    public GameObject icon;
    public RectTransform cooldown;
    public TextMeshProUGUI manacost;
    public TextMeshProUGUI damage;
    public GameObject highlight;
    public Spell spell;
    float last_text_update;
    const float UPDATE_DELAY = 1;
    public GameObject dropbutton;
    //public int selfindex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        last_text_update = 0;
        highlight.SetActive(false);
        EventBus.Instance.OnSpellSolo += HideDropButton;
        EventBus.Instance.OnSpellMultiple += ShowDropButton;
    }

    public void SetSpell(Spell spell)
    {
        this.spell = spell;
        GameManager.Instance.spellIconManager.PlaceSprite(spell.GetIcon(), icon.GetComponent<Image>());
        manacost.text = spell.GetManaCost().ToString();
        damage.text = spell.GetDamage().ToString();
    }
    public void RemoveSpell() {
        this.spell = null;
        EventBus.Instance.OnSpellRemoveEffect(spell, transform.GetSiblingIndex());
    }
    public void Highlight() {
        highlight.SetActive(true);
    }
    public void UnHighlight() {
        highlight.SetActive(false);
    }
    public void ShowDropButton(int ahhhh) { dropbutton.SetActive(true); Debug.Log("Showing drop button for " + spell.GetName()); }
    public void HideDropButton(SpellCaster single) { dropbutton.SetActive(false); }
    // Update is called once per frame
    void Update()
    {
        if (spell == null) return;
        if (Time.time > last_text_update + UPDATE_DELAY)
        {
            manacost.text = spell.GetManaCost().ToString();
            damage.text = spell.GetDamage().ToString();
            last_text_update = Time.time;
        }
        
        float since_last = Time.time - spell.last_cast;
        float perc;
        if (since_last > spell.GetCooldown())
        {
            perc = 0;
        }
        else
        {
            perc = 1-since_last / spell.GetCooldown();
        }
        cooldown.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 48 * perc);
    }
}

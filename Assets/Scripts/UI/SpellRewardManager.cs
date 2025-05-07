using UnityEngine;
using TMPro;

public class SpellRewardManager : MonoBehaviour
{
    public TextMeshProUGUI label;
    public SpellCaster player;
    public string spell;
    public RewardScreenManager rsw;
    

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pickSpell() {
        var sb =  new SpellBuilder();
        int rnd = (int) Mathf.Floor(Random.value* sb.spellTypes.Length);
        spell = sb.spellTypes[rnd];
        label.text = spell;
    }

    public void addSpell() {
        if(spell == null) {
            return;
        }
        var res = player.addSpell(spell);
        rsw.DestroyButtons();
    }

    
}

using UnityEngine;
using TMPro;

public class SpellRewardManager : MonoBehaviour
{
    public TextMeshProUGUI label;
    public SpellCaster player;
    public string spell;
    public RewardScreenManager rsw;

    const int modifierRange = 2;
    

    
    // Start is called once before the first exeution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pickSpell() {
        var sb =  new SpellBuilder();
        spell = "";
        //choose number of mods
        int mods = (int) Mathf.Floor(Random.value * modifierRange);
        //for every modifier, pick a random one and concat it to spell
        for(int i = 0; i < mods; i++) {
            int rnd = (int) Mathf.Floor(Random.value * sb.modifierTypes.Length);
            spell = spell + sb.modifierTypes[rnd] + " ";
        }
        //concat spell type to spell
        int s = (int) Mathf.Floor(Random.value* sb.spellTypes.Length);
        spell = spell + sb.spellTypes[s];
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

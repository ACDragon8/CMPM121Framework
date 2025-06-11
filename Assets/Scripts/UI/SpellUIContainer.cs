using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SpellUIContainer : MonoBehaviour
{
    public GameObject[] spellUIs;
    public PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventBus.Instance.OnSpellPickup += DisplayNewSpell;
        EventBus.Instance.OnSpellRemove += RemoveSpell;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisplayNewSpell(Spell spell, int index) {
        spellUIs[index].GetComponent<SpellUI>().SetSpell(spell);
        spellUIs[index].SetActive(true);
    }
    public void RemoveSpell(Spell spell, int index) {
        spellUIs[index].SetActive(false);
        //TODO if extra time, make the spell UI slide over instead of leaving gaps where there used to be spells
    }
    public void HighlightCurrSpell(int index) {
        //There's probably a better way to do this but idk how
        spellUIs[index].GetComponent<SpellUI>().Highlight();
        for (int i = 0; i < spellUIs.Length; i++) {
            if (spellUIs[i].activeSelf && i != index) {
                spellUIs[i].GetComponent<SpellUI>().UnHighlight();
            }
        }
    }
}

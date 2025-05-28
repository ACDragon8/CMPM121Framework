using TMPro;
using UnityEngine;

public class CharacterSelectorController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI label;
    public string character_class;
    public EnemySpawner spawner;
    void Start()
    {
        //label = new TextMeshProUGUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetCharacter(string text)
    {
        character_class = text;
        label.text = text;
    }

    public void SelectCharacter()
    {
        spawner.SelectCharacter(character_class);
    }
}

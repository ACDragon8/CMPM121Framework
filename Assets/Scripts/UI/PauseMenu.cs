using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public EnemySpawner spawner;
    public GameObject spellPanel;
    public SpellUIContainer spellUIContainer;
    public TextMeshProUGUI[] spellTexts;
    public HorizontalLayoutGroup spellLayoutGroup;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.state == GameManager.GameState.PAUSED)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        // Count active spells
        int activeSpells = 0;
        for (int i = 0; i < spellUIContainer.spellUIs.Length; i++)
        {
            SpellUI spellUIComponent = spellUIContainer.spellUIs[i].GetComponent<SpellUI>();
            if (spellUIComponent.spell != null)
            {
                activeSpells++;
            }
        }

        // Adjust spacing based on active spells
        if (activeSpells == 2)
        {

            spellLayoutGroup.spacing = -650;
        }
        else if (activeSpells == 3)
        {
            spellLayoutGroup.spacing = -300;
        }
        else if (activeSpells == 4)
        {
            spellLayoutGroup.spacing = 40;
        }
    }

    private void UpdateSpellPanel()
    {
        // Clear all text elements
        foreach (var text in spellTexts)
        {
            text.text = "";
        }

        // Clear only the spell-specific UI elements
        foreach (Transform child in spellPanel.transform)
        {
            SpellUI spellUIComponent = child.GetComponent<SpellUI>();
            if (spellUIComponent != null)
            {
                Destroy(child.gameObject);
            }
        }

        // Display active spells
        for (int i = 0; i < spellUIContainer.spellUIs.Length && i < spellTexts.Length; i++)
        {
            SpellUI spellUIComponent = spellUIContainer.spellUIs[i].GetComponent<SpellUI>();
            if (spellUIComponent.spell != null) // Check if the spell is active
            {
                // Instantiate spell UI
                GameObject spellUI = Instantiate(spellUIContainer.spellUIs[i], spellPanel.transform);
                spellUI.SetActive(true);

                // Set text description
                spellTexts[i].text = $"{spellUIComponent.spell.GetName()}\n" +
                                     $"Mana Cost: {spellUIComponent.spell.GetManaCost()}\n" +
                                     $"Damage: {spellUIComponent.spell.GetDamage()}\n" +
                                     $"{spellUIComponent.spell.GetDescription()}";
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        UpdateSpellPanel();
        Time.timeScale = 0;
        GameManager.Instance.state = GameManager.GameState.PAUSED;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        GameManager.Instance.state = GameManager.GameState.INWAVE;
    }

    public void MainMenu()
    {
        Debug.Log("MainMenu");
        StatsManager.Instance.ResetEnemyKills();
        StatsManager.Instance.ResetWaveNum();
        GameManager.Instance.state = GameManager.GameState.GAMEOVER;
        GameManager.Instance.RemoveAllEnemies();
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        spawner.Restart();
    }
}

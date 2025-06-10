using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public EnemySpawner spawner;
    public GameObject spellPanel;
    public SpellUIContainer spellUIContainer;
    

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
    }

    private void UpdateSpellPanel()
    {
        // Clear existing spell UI elements
        foreach (Transform child in spellPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Display only active spells
        for (int i = 0; i < spellUIContainer.spellUIs.Length; i++)
        {
            SpellUI spellUIComponent = spellUIContainer.spellUIs[i].GetComponent<SpellUI>();
            if (spellUIComponent.spell != null) // Check if the spell is active
            {
                GameObject spellUI = Instantiate(spellUIContainer.spellUIs[i], spellPanel.transform);
                spellUI.SetActive(true);
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

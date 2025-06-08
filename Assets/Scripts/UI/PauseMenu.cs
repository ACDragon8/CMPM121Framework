using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public EnemySpawner spawner;
    

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

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
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

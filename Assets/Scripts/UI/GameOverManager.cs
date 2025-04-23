using UnityEngine;

public class GameOverMan : MonoBehaviour
    
{
    public GameObject GameOver;
    public GameObject StartGame;
    public EnemySpawner spawner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.GAMEOVER)
        {
            GameOver.SetActive(true);
            StartGame.SetActive(true);
        }
        else
        {
            GameOver.SetActive(false);
            StartGame.SetActive(false);
        }
    }

    public void Restart() {
        StatsManager.Instance.ResetEnemyKills();
        StatsManager.Instance.ResetWaveNum();
        GameManager.Instance.state = GameManager.GameState.PREGAME;
        spawner.Restart();
    }
}

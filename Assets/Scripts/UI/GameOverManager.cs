using UnityEngine;

public class GameOverMan : MonoBehaviour
    
{
    public GameObject GameOver;
    public GameObject StartGame;
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
            StartGame.SetActive(false);
        }
        else if (GameManager.Instance.state == GameManager.GameState.PREGAME) {
            StartGame.SetActive(true);
            GameOver.SetActive(false);
        }
        else
        {
            GameOver.SetActive(false);
            StartGame.SetActive(false);
        }
    }

    public void Restart() {
        GameManager.Instance.state = GameManager.GameState.PREGAME;
    }
}

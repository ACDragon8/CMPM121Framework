using UnityEngine;

public class GameOverMan : MonoBehaviour
    
{
    public GameObject GameStart;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.GAMEOVER)
        {
            GameStart.SetActive(true);
        }
        else
        {
            GameStart.SetActive(false);
        }
    }

    void Restart() {
        GameManager.Instance.state = GameManager.GameState.PREGAME;
    }
}

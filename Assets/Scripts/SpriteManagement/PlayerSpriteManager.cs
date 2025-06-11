using UnityEngine;
using UnityEngine.UI;

public class PlayerSpriteManager : IconManager
{
  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GameManager.Instance.playerSpriteManager = this;
    }

}

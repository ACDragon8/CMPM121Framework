using UnityEngine;

public class RelicIconManager : IconManager
{
    void Awake()
    {
        GameManager.Instance.relicIconManager = this;
    }
    //This is the code responsible for displaying the relics on the top of the screen?
}

using UnityEngine;
using System;
using System.Collections.Generic;
public class TrainingRoomEventbus 
{
    public RoomState state;
    public enum RoomState
    {
        MENU,
        COMBAT
    }
    private static TrainingRoomEventbus theInstance;
    public static TrainingRoomEventbus Instance {
        get 
        { 
            if (theInstance == null)
                theInstance = new TrainingRoomEventbus();
            return theInstance; 
        }
    }
    public GameObject player;

    //This just works to keep track of how many enemies are out there
    private List<GameObject> enemies;
    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
    public void RemoveAllEnemies() {
        foreach (GameObject enemy in enemies) {
            enemy.GetComponent<EnemyController>().Die();
        }
    }

    public Action OpenMenu;
    public Action CloseMenu;
    public void OnOpenMenu() 
    {
        OpenMenu?.Invoke();
    }
    public void OnCloseMenu()
    {
        CloseMenu?.Invoke();
    }

    public Action<Spell> SelectBaseSpell;
    public Action SpellCrafted;
    public Action SpellListClosed;

    public void onSelectBaseSpell(Spell s) {
        SelectBaseSpell?.Invoke(s);
    }
    public void onSpellCrafted() {
        SpellCrafted?.Invoke();
    }
    public void onSpellListClosed() {
        SpellListClosed?.Invoke();
    }
}



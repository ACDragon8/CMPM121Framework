using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private TrainingRoomEventbus() {
        enemies = new List<GameObject>();
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
        while (enemies.Count > 0) {
            GameObject enemy = enemies[0];
            enemy.GetComponent<EnemyController>().Die();
            RemoveEnemy(enemy);
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



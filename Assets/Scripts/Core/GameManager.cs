using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameManager 
{
    public enum GameState
    {
        PREGAME,
        INWAVE,
        PAUSED,
        WAVEEND,
        COUNTDOWN,
        GAMEOVER,
        GAMEWIN
    }
    public GameState state;

    public int countdown;
    private static GameManager theInstance;
    public static GameManager Instance {  get
        {
            if (theInstance == null)
                theInstance = new GameManager();
            return theInstance;
        }
    }

    public GameObject player;
    
    public ProjectileManager projectileManager;
    public SpellIconManager spellIconManager;
    public EnemySpriteManager enemySpriteManager;
    public PlayerSpriteManager playerSpriteManager;
    public RelicIconManager relicIconManager;

    private List<GameObject> enemies;
    public int enemy_count { get { return enemies.Count; } }

    public event Action LevelStart;
    public event Action OnWaveEnd;
    public event Action OnRewardSelectionFinished;
    public event Action OnPlayerDeath;

    public void OnPlayerDeathEffects() {
        OnPlayerDeath?.Invoke();
    }
    public void OnWaveEndEffects() {
        OnWaveEnd?.Invoke();
    }
    public void LevelStartEffects() {
        LevelStart?.Invoke();
    }
    public void OnRewardSelectionFinishedEffects() {
        OnRewardSelectionFinished?.Invoke();
    }
    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public void RemoveAllEnemies()
    {
        GameObject[] enemyArray = enemies.ToArray();
        foreach (GameObject enemy in enemyArray)
        {
            GameObject.Destroy(enemy);
        }
        enemies.Clear();
    }

    public GameObject GetClosestEnemy(Vector3 point)
    {
        if (enemies == null || enemies.Count == 0) return null;
        if (enemies.Count == 1) return enemies[0];
        return enemies.Aggregate((a,b) => (a.transform.position - point).sqrMagnitude < (b.transform.position - point).sqrMagnitude ? a : b);
    }

    private GameManager()
    {
        enemies = new List<GameObject>();
    }
}

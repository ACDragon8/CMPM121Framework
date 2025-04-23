using System.Collections.Generic;
using UnityEngine;

public class StatsManager
{
    /* Supposedly good practice to keep field values private
     * and have methods to access and modify them.
    */
    private int waveNum;
    private int totalWaves; //This one is only applicable if not endless
    private string levelName;
    private Dictionary<string, int> enemyKills;
    private static StatsManager theInstance;
    public static StatsManager Instance
    {
        get
        {
            if (theInstance == null)
                theInstance = new StatsManager();
            return theInstance;
        }
    }
    private StatsManager() 
    { 
        waveNum = 0;
        totalWaves = 0;
        enemyKills = new Dictionary<string, int>();
    }
    public void UpdateWaveNum(int newNum)
    {
        waveNum = newNum;
    }
    public int GetWaveNum()
    {
        return waveNum;
    }
    public void ResetWaveNum()
    {
        waveNum = 0;
    }
    public void UpdateTotalWaves(int newTotalWaves)
    {
        totalWaves = newTotalWaves;
    }
    public void SetLevelName(string name)
    {
        levelName = name;
    }
    public string getLevelName() 
    {
        return levelName;
    }
    public void ResetEnemyKills() 
    {
        foreach (var item in enemyKills) 
        {
            enemyKills[item.Key] = 0;
        }
    }
    public Dictionary<string, int> GetEnemiesKilled() 
    {
        return enemyKills;
    }
    public void addKill(string enemy, int increment) 
    {
        //If key exists, add increment to value
        //Else create new entry and set it to increment
    }
}

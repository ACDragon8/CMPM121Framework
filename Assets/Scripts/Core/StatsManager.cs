using System.Collections.Generic;
using UnityEngine;

public class StatsManager
{
    /* Supposedly good practice to keep field values private
     * and have methods to access and modify them.
     * Can use get/set properties instead of all these methods?
    */
    public int waveNum { get; set; }
    public int totalWaves { get; set; } //This one is only applicable if not endless
    public string levelName { get; set; }
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
    public void ResetWaveNum()
    {
        waveNum = 0;
    }
    //This function below only should be used if not endless
    public void UpdateTotalWaves(int newTotalWaves)
    {
        totalWaves = newTotalWaves;
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

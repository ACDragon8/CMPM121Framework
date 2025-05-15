using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System;

//test

public class EnemySpawner : MonoBehaviour
{
    public Image level_selector;
    public GameObject button;
    public SpawnPoint[] SpawnPoints; 
    public GameObject enemy;
    public Dictionary<string, EnemyType> enemy_list;
    public Dictionary<string, LevelData> level_list;
    public string level;
    public int WaveCount;
    public bool cancel;
    public Dictionary<string, GameObject> level_buttons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        WaveCount = 0;
        cancel = false;
        
        //setup enemy types
        enemy_list = new Dictionary<string, EnemyType>();
        var enemytext = Resources.Load<TextAsset>("enemies");

        JToken jo = JToken.Parse(enemytext.text);
        foreach(var e in jo) {
            EnemyType en = e.ToObject<EnemyType>();
            enemy_list[en.name] = en;
        }


        //setup level types
        level_list = new Dictionary<string, LevelData>();
        var leveltext = Resources.Load<TextAsset>("levels");

        jo = JToken.Parse(leveltext.text);
        foreach(var l in jo) {
            LevelData lv = l.ToObject<LevelData>();
            level_list[lv.name] = lv;
        }
        //TODO figure out how to get the height of the menu and maybe create a separate class
        //for these UI things
        int spacing = 120/level_list.Count;
        int i = level_list.Count;
        level_buttons = new Dictionary<string, GameObject>();
        foreach (var difficulty in level_list) { 
            GameObject b = Instantiate(button, level_selector.transform);
            b.transform.localPosition = new Vector3(0, spacing*i - 50);
            b.GetComponent<MenuSelectorController>().spawner = this;
            b.GetComponent<MenuSelectorController>().SetLevel(difficulty.Key);
            level_buttons[difficulty.Key] = b;
            i--;
            Debug.Log("Created button for " + difficulty.Key);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.GAMEOVER) {
            this.Cancel();
            WaveCount = 0;
        }
    }

    public void Restart() //turn to event -------------------------------------------------------------------
    {
        level_selector.gameObject.SetActive(true);
    }
    public void Cancel() //turn to event??
    {
        cancel = true;
        /* The way the code is set up, it will either cancel the current wave being spawned by SpawnWave()
         * or if there is no ongoing waves, it will cancel the next wave created by SpawnWave()
         */
    }
    public void StartLevel(string levelname) // definitely turn to event ----------------------------------
    {
        level = levelname;
        WaveCount = 0;
        cancel = false;
        level_selector.gameObject.SetActive(false);
        StatsManager.Instance.levelName = levelname;
        // this is not nice: we should not have to be required to tell the player directly that the level is starting
        GameManager.Instance.player.GetComponent<PlayerController>().StartLevel();
        StartCoroutine(SpawnWave());
    }

    public void NextWave() // absolutely turn to event ------------------------------
    {
        //this sucks but deadlines are deadlines
        ReversePolishCalc calc = new ReversePolishCalc();
        string a = "95 wave 5 * +";
        string[] b = a.Split(' ');
        int index = 0;
        foreach (var item in b) {
            if (item == "wave")
            {
                b[index] = WaveCount.ToString();
            }
            index++;
        }
        int c = ReversePolishCalc.Calculate(b);
        GameManager.Instance.player.GetComponent<PlayerController>().hp.SetMaxHP(c);
        a = "90 wave 10 * +";
        b = a.Split();
        index = 0;
        foreach (var item in b) {
            if (item == "wave")
            {
                b[index] = WaveCount.ToString();
            }
            index++;
        }
        c = ReversePolishCalc.Calculate(b);
        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.SetMaxMana(c);
        a = "10 wave +";
        b = a.Split();
        index = 0;
        foreach (var item in b) {
            if (item == "wave")
            {
                b[index] = WaveCount.ToString();
            }
            index++;
        }
        c = ReversePolishCalc.Calculate(b);
        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.SetManaRegen(c);
        a = "wave 10 *";
        b = a.Split();
        index = 0;
        foreach (var item in b) {
            if (item == "wave")
            {
                b[index] = WaveCount.ToString();
            }
            index++;
        }
        c = ReversePolishCalc.Calculate(b);
        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.SetSpellPower(c);
        GameManager.Instance.player.GetComponent<PlayerController>().SetSpeed(5);
       
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        WaveCount++;
        StatsManager.Instance.waveNum = WaveCount;
        LevelData lvl = level_list[level];
        int delay = 0;
        int[] sequence = new int[1];

        GameManager.Instance.state = GameManager.GameState.COUNTDOWN;
            GameManager.Instance.countdown = 3;
            for (int i = 3; i > 0; i--)
            {
                yield return new WaitForSeconds(1);
                GameManager.Instance.countdown--;
            }
            
                GameManager.Instance.state = GameManager.GameState.INWAVE;

        //This step parses the info in the JSON
        foreach (var spawn in lvl.spawns) {
            //Enemy, count, hp and location are always there but delay & sequence might not be
            //The catch portions show the default values if missing
            try { delay = Int32.Parse(spawn.delay); } catch { delay = 1; }
            if (!(spawn.sequence != null))
            {
                Debug.Log(spawn.enemy + " is missing a sequence order. Setting to default");
                sequence[0] = 1;
            }
            else {
                sequence = spawn.sequence;
            } 

            //Convert words into (string) digits for RPN calc
            //This is for the new base hp
            string[] hp_split = spawn.hp.Split(' ');
            int index = 0;
            foreach (var item in hp_split)
            {
                if (item == "base")
                {
                    
                    hp_split[index] = enemy_list[spawn.enemy].hp.ToString();
                }
                else if (item == "wave")
                {
                    
                    hp_split[index] = WaveCount.ToString();
                }
                index++;
            }
            int new_hp = ReversePolishCalc.Calculate(hp_split);

            //This is for total amount of enemies to spawn
            string[] count_split = spawn.count.Split(' ');
            index = 0;
            foreach (var item in count_split) {
                if (item == "wave")
                {
                    count_split[index] = WaveCount.ToString();
                }
                index++;
            }
            int total_count = ReversePolishCalc.Calculate(count_split);
            int curr_spawned = 0;
            bool has_sequence = false;
            if (sequence.Length > 1) {
                has_sequence = true;
            }
            int sequence_index = 0;

            while (total_count > curr_spawned)
            {
                
                for (int i = 0; i < sequence[sequence_index]; i++) {
                    yield return Spawn(spawn.enemy, new_hp);
                    if (cancel) { break; }
                    curr_spawned++;
                    if (curr_spawned == total_count) { break; }
                }
                if (cancel) { break; }
                if (has_sequence)
                {
                    sequence_index++;
                    sequence_index = sequence_index % sequence.Length;
                }
                yield return new WaitForSeconds(delay);
            }
        }
        Debug.Log("Done spawning wave!");
        if (cancel) { Debug.Log("Wave got cancelled."); } //turn game over into an event --------------------------------
        yield return new WaitWhile(() => GameManager.Instance.enemy_count > 0);
        if (GameManager.Instance.state != GameManager.GameState.GAMEOVER) {
            GameManager.Instance.state = GameManager.GameState.WAVEEND;
        }
    }
    /*IEnumerator SpawnZombie()
    {
        SpawnPoint spawn_point = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        Vector2 offset = Random.insideUnitCircle * 1.8f;
                
        Vector3 initial_position = spawn_point.transform.position + new Vector3(offset.x, offset.y, 0);
        GameObject new_enemy = Instantiate(enemy, initial_position, Quaternion.identity);

        new_enemy.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.enemySpriteManager.Get(0);
        EnemyController en = new_enemy.GetComponent<EnemyController>();
        en.hp = new Hittable(50, Hittable.Team.MONSTERS, new_enemy);
        en.speed = 10;
        GameManager.Instance.AddEnemy(new_enemy);
        yield return new WaitForSeconds(0.5f);
    }*/

    IEnumerator Spawn(string enemy_name, int new_hp)
    {
        //pick enemey type
        EnemyType enemy_type = enemy_list[enemy_name];
        //pick spawnpoint
        SpawnPoint spawn_point = SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Length)];
        Vector2 offset = UnityEngine.Random.insideUnitCircle * 1.8f;
                
        Vector3 initial_position = spawn_point.transform.position + new Vector3(offset.x, offset.y, 0);
        GameObject new_enemy = Instantiate(enemy, initial_position, Quaternion.identity);

        new_enemy.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.enemySpriteManager.Get(enemy_type.sprite);
        EnemyController en = new_enemy.GetComponent<EnemyController>();
        en.hp = new Hittable(new_hp, Hittable.Team.MONSTERS, new_enemy);
        en.speed = enemy_type.speed;
        en.damage = enemy_type.damage;
        GameManager.Instance.AddEnemy(new_enemy);
        yield return new WaitForSeconds(0.5f);
    }
}

public class EnemyType {
    public string name;
    public int sprite;
    public int hp;
    public int speed;
    public int damage;
}

public class LevelData {
    public string name;
    public int waves;
    //public string[] spawns;
    public SpawnData[] spawns;
    //TODO: parse jsson object of spawns to turn into wave spawning info
    public bool process_spawn_data() {
        return true;
    }
}

public class SpawnData {
    public string enemy;
    public string count;
    public string hp;
    public string delay;
    public int[] sequence;
    public string location;
}

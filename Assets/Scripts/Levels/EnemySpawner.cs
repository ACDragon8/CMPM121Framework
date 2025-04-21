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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject selector_eas = Instantiate(button, level_selector.transform);
        selector_eas.transform.localPosition = new Vector3(0, 110);
        selector_eas.GetComponent<MenuSelectorController>().spawner = this;
        selector_eas.GetComponent<MenuSelectorController>().SetLevel("Easy");

        GameObject selector_med = Instantiate(button, level_selector.transform);
        selector_med.transform.localPosition = new Vector3(0, 60);
        selector_med.GetComponent<MenuSelectorController>().spawner = this;
        selector_med.GetComponent<MenuSelectorController>().SetLevel("Medium");

        GameObject selector_har = Instantiate(button, level_selector.transform);
        selector_har.transform.localPosition = new Vector3(0, 0);
        selector_har.GetComponent<MenuSelectorController>().spawner = this;
        selector_har.GetComponent<MenuSelectorController>().SetLevel("Endless");

        WaveCount = 0;
        
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel(string levelname)
    {
        level = levelname;
        level_selector.gameObject.SetActive(false);
        //Debug.Log(levelname);
        // this is not nice: we should not have to be required to tell the player directly that the level is starting
        GameManager.Instance.player.GetComponent<PlayerController>().StartLevel();
        StartCoroutine(SpawnWave());
    }

    public void NextWave()
    {
        StartCoroutine(SpawnWave());
    }

    //TODO implement the info from the json here
    IEnumerator SpawnWave()
    {
        WaveCount++;
        Debug.Log("In the spawnWave func now");
        LevelData lvl = level_list[level];
        ReversePolishCalc RPN = new ReversePolishCalc();
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
            int new_hp = RPN.Calculate(hp_split);

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
            int total_count = RPN.Calculate(count_split);
            int curr_spawned = 0;
            bool has_sequence = false;
            if (sequence.Length > 1) {
                has_sequence = true;
            }
            int sequence_index = 0;

            while (total_count > curr_spawned)
            {
                /*
                GameManager.Instance.state = GameManager.GameState.COUNTDOWN;
                GameManager.Instance.countdown = delay;
                for (int i = delay; i > 0; i--) {
                    yield return new WaitForSeconds(1);
                    GameManager.Instance.countdown--;
                }*/

                
                for (int i = 0; i < sequence[sequence_index]; i++) {
                    //TODO modify Spawn() to work w/ new base hp
                    yield return Spawn(spawn.enemy, new_hp);
                    curr_spawned++;
                    if (curr_spawned == total_count) { break; }
                }
                if (has_sequence)
                {
                    sequence_index++;
                    sequence_index = sequence_index % sequence.Length;
                }
                yield return new WaitForSeconds(delay);
            }
        }
        Debug.Log("Done spawning wave!");
        yield return new WaitWhile(() => GameManager.Instance.enemy_count > 0);
        GameManager.Instance.state = GameManager.GameState.WAVEEND;
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

    //TODO modify Spawn() to take into account new base HP
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

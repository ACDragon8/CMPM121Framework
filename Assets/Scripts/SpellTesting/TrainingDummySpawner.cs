using UnityEngine;


//test

public class TrainingDummySpawner : MonoBehaviour
{
    public GameObject enemy;
    public SpawnPoint[] SpawnPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TrainingRoomEventbus.Instance.CloseMenu += Spawn;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Spawn() {
        SpawnGroup(100);
    }
    public void SpawnGroup(int hp)
    {
        SpawnPoint spawn_point = SpawnPoints[0];
        int x = -1;
        int y = 1;
        for (int i = 0; i < 4; i++)
        {
            Vector3 initial_position = spawn_point.transform.position + new Vector3(2 * x, 2 * y, 0);
            GameObject new_enemy = Instantiate(enemy);
            new_enemy.transform.position = initial_position;
            new_enemy.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.enemySpriteManager.Get(0);
            EnemyController en = new_enemy.GetComponent<EnemyController>();
            en.hp = new Hittable(hp, Hittable.Team.MONSTERS, new_enemy);
            en.speed = 0;
            en.damage = 1;
            GameManager.Instance.AddEnemy(new_enemy);
            if (i % 2 == 0)
            {
                x *= -1;
            }
            else 
            { 
                y *= -1;
            }
        }
    }

}



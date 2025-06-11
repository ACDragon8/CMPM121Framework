using UnityEngine;
using System;
using System.Collections;


public class EnemyController : MonoBehaviour
{

    public Transform target;
    public int speed;
    public Hittable hp;
    public HealthBar healthui;
    public int damage;
    public bool dead;
    public bool slow_immunity;

    public float last_attack;
    public GameObject[] dropItemPrefabs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameManager.Instance.player.transform;
        hp.OnDeath += Die;
        healthui.SetHealth(hp);
        slow_immunity = false;

        hp.OnDeath += TryDropItem;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.GAMEOVER)
        {
            Die();
        }
        Vector3 direction = target.position - transform.position;
        if (direction.magnitude < 2f)
        {
            DoAttack();
        }
        else
        {
            GetComponent<Unit>().movement = direction.normalized * speed;
        }
    }

    void DoAttack()
    {
        if (last_attack + 2 < Time.time)
        {
            last_attack = Time.time;
            target.gameObject.GetComponent<PlayerController>().hp.Damage(new Damage(damage, Damage.Type.PHYSICAL));
        }
    }


    public void Die()
    {
        if (!dead)
        {
            dead = true;
            GameManager.Instance.RemoveEnemy(gameObject);
            Destroy(gameObject);
        }
    }

    public void ModifySpeed(float percent, int duration)
    {
        CoroutineManager.Instance.StartCoroutine(SetSpeed(percent, duration));
    }
    public IEnumerator SetSpeed(float percent, int duration)
    {
        if (!slow_immunity)
        {
            slow_immunity = true;
            int old_speed = speed;
            float new_speed = speed * percent;
            speed = (int)new_speed;
            yield return new WaitForSeconds(duration);
            speed = old_speed;
        }
        yield break;
    }

    //Drop item on death with a 35% chance
    void TryDropItem()
    {
        float chance = UnityEngine.Random.value; // Returns 0.0 to 1.0

        if (chance <= 0.35f) // 35% chance
        {
            int index = UnityEngine.Random.Range(0, dropItemPrefabs.Length);
            GameObject item = Instantiate(dropItemPrefabs[index], transform.position, Quaternion.identity);
        }
    }
}

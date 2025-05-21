using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using UnityEngine.Rendering;
using NUnit.Framework;
using System.Collections.Generic;

public class ProjectileController : MonoBehaviour
{
    public float lifetime;
    public event Action<Hittable,Vector3> OnHit;
    public ProjectileMovement movement;
    public Vector3 prevPos;
    public Vector3 moveDir;
    public bool knockback;
    public bool pierce;
    public List<Collider2D> past_hits;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (knockback)
        {
            prevPos = transform.position;
            StartCoroutine(SampleDirection());
        }
        if (pierce) {
            past_hits = new List<Collider2D>();
            this.GetComponent<CircleCollider2D>().isTrigger = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = transform.position - prevPos;
        movement.Movement(transform);
        prevPos = transform.position;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("projectile")) return;
        if (collision.gameObject.CompareTag("unit"))
        {
            var ec = collision.gameObject.GetComponent<EnemyController>();
            if (ec != null)
            {
                OnHit(ec.hp, transform.position);
            }
            else
            {
                var pc = collision.gameObject.GetComponent<PlayerController>();
                if (pc != null)
                {
                    OnHit(pc.hp, transform.position);
                }
            }
            if (knockback) { collision.gameObject.transform.Translate(moveDir); }
        }
        Destroy(gameObject);
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("projectile")) return;
        if (collision.gameObject.CompareTag("unit"))
        {
            if (pierce && CheckInHits(collision))
            {
                Debug.Log("I've seen this dude before");
                return;
            }
            var ec = collision.gameObject.GetComponent<EnemyController>();
            if (ec != null)
            {
                OnHit(ec.hp, transform.position);
            }
            else
            {
                var pc = collision.gameObject.GetComponent<PlayerController>();
                if (pc != null)
                {
                    OnHit(pc.hp, transform.position);
                }
            }
            if (knockback) { collision.gameObject.transform.Translate(moveDir); }
            if (pierce)
            {
                past_hits.Add(collision);
                StartCoroutine(HitImmunity());
                return;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool CheckInHits(Collider2D fresh) {
        foreach (var hit in past_hits) {
            if (fresh == hit) { return true; }
        }
        return false;
    }
    
    IEnumerator HitImmunity() 
    {
        yield return new WaitForSeconds(0.2f);
        past_hits.RemoveAt(0);  
    }
    public void SetLifetime(float lifetime)
    {
        StartCoroutine(Expire(lifetime));
    }

    IEnumerator Expire(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
    public void GetDirection() {
        moveDir = transform.position - prevPos;
        moveDir.Normalize();
        prevPos = transform.position;
    }
    IEnumerator SampleDirection() {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            GetDirection();
        }
    }
}

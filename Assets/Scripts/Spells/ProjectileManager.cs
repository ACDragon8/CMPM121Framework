using UnityEngine;
using System;
using System.Collections.Generic;

public class ProjectileManager : MonoBehaviour
{
    public GameObject[] projectiles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.projectileManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //TODO rather than having projectile altering modifiers be individual booleans
    //make it accept a var that can be an empty list?
    //This list will have lifetime, piercing, knockback and whatnot
    public void CreateProjectile(int which, string trajectory, Vector3 where, Vector3 direction, float speed, List<Action<Hittable, Vector3>> onHit, bool pierce = false, bool knockback = false, float lifetime=0.0f)
    {
        GameObject new_projectile = Instantiate(projectiles[which], where + direction.normalized * 1.1f, Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
        new_projectile.GetComponent<ProjectileController>().movement = MakeMovement(trajectory, speed);
        foreach(Action<Hittable, Vector3> hitMethod in onHit)
        {
            new_projectile.GetComponent<ProjectileController>().OnHit += hitMethod;
        }
        new_projectile.GetComponent<ProjectileController>().pierce = pierce;
        new_projectile.GetComponent<ProjectileController>().knockback = knockback;
        if (lifetime != 0.0f) { new_projectile.GetComponent<ProjectileController>().SetLifetime(lifetime); }
    }
    public void CreateProjectile(int which, string trajectory, Vector3 where, Vector3 direction, float speed, List<Action<Hittable, Vector3>> onHit, float lifetime)
    {
        GameObject new_projectile = Instantiate(projectiles[which], where + direction.normalized * 1.1f, Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
        new_projectile.GetComponent<ProjectileController>().movement = MakeMovement(trajectory, speed);
        foreach (Action<Hittable, Vector3> hitMethod in onHit)
        {
            new_projectile.GetComponent<ProjectileController>().OnHit += hitMethod;
        }
        if (lifetime != 0.0f) { new_projectile.GetComponent<ProjectileController>().SetLifetime(lifetime); }
    }
    /*
    public void CreateProjectile(int which, string trajectory, Vector3 where, Vector3 direction, float speed, List<Action<Hittable, Vector3>> onHit, Dictionary<string, bool> onHitEffects=null, float lifetime = 0.0f)
    {
        GameObject new_projectile = Instantiate(projectiles[which], where + direction.normalized * 1.1f, Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
        new_projectile.GetComponent<ProjectileController>().movement = MakeMovement(trajectory, speed);
        new_projectile.GetComponent<ProjectileController>().OnHit += onHit;
        
        if (lifetime != 0.0f) { new_projectile.GetComponent<ProjectileController>().SetLifetime(lifetime); }
    }*/

    public ProjectileMovement MakeMovement(string name, float speed)
    {
        if (name == "straight")
        {
            return new StraightProjectileMovement(speed);
        }
        if (name == "homing")
        {
            return new HomingProjectileMovement(speed);
        }
        if (name == "spiraling")
        {
            return new SpiralingProjectileMovement(speed);
        }
        return null;
    }

}

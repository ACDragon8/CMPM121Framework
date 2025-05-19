using UnityEngine;
using System;

public class EventBus 
{
    private static EventBus theInstance;
    public static EventBus Instance
    {
        get
        {
            if (theInstance == null)
                theInstance = new EventBus();
            return theInstance;
        }
    }

    public event Action<Vector3, Damage, Hittable> OnDamage;
    public event Action<Vector3, Hittable> OnDeath;
    public event Action<Spell, int> OnSpellPickup;
    public event Action<Spell, int> OnSpellRemove;
    
    public void DoDamage(Vector3 where, Damage dmg, Hittable target)
    {
        OnDamage?.Invoke(where, dmg, target);
    }
    public void OnDeathEffect(Vector3 where, Hittable target) {
        OnDeath?.Invoke(where, target);
    }

    //Index just tells it which slot it should be when looking at SpellCaster
    public void OnSpellPickupEffect(Spell spell, int index) {
        OnSpellPickup?.Invoke(spell, index);
    }

    public void OnSpellRemoveEffect(Spell spell, int index) {
        OnSpellRemove?.Invoke(spell, index);
    }

}

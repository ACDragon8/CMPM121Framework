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
    public event Action Cast;
    public event Action Cat;
    public event Action Idle;
    public event Action Move;

    public event Action SwitchSpell;

    public void DoDamage(Vector3 where, Damage dmg, Hittable target)
    {
        OnDamage?.Invoke(where, dmg, target);
    }

    public void OnDeathEffect(Vector3 where, Hittable target)
    {
        OnDeath?.Invoke(where, target);
    }

    public void OnIdle()
    {
        Idle?.Invoke();
    }

    public void OnMove()
    {
        Move?.Invoke();
    }
    public void BeforeCat()
    {
        Cat?.Invoke();
    }

    public void OnCast()
    {
        Cast?.Invoke();
    }

    public void OnSwitchSpell()
    {
        SwitchSpell?.Invoke();
    }

    /*
     * Everything below was just to help Cynthia
     * coordinate UI stuff because UI sucks
     */
    //Spell UI stuff
    public event Action<Spell, int> OnSpellPickup;
    public event Action<Spell, int> OnSpellRemove;
    public event Action<SpellCaster> OnSpellSolo;
    public event Action<int> OnSpellMultiple;
    public void OnSpellPickupEffect(Spell spell, int index) {
        OnSpellPickup?.Invoke(spell, index);
    }

    public void OnSpellRemoveEffect(Spell spell, int index) {
        OnSpellRemove?.Invoke(spell, index);
    }
    public void OnSpellSoloEffect(SpellCaster singleSpell) {
        OnSpellSolo?.Invoke(singleSpell);
    }
    public void OnSpellMultipleEffect(int amount) {
        OnSpellMultiple?.Invoke(amount);
    }
    /*
     * And here are the events for dealing with Relic UI
     * Dragon, curse you for not having the courage to mess with UI or do it properly >.<
     */
    public event Action<Relic, int> OnRelicPickup;
    public void OnRelicPickupEffect(Relic r, int index)
    {
        OnRelicPickup?.Invoke(r, index);
    }
    public event Action<Spell> OnSpellCrafted;
    public event Action<Relic> OnRelicCrafted;
    public event Action<SpellCaster> OnSpellCasterInitialized;
    public void OnSpellCraftedEffect(Spell sp) {
        OnSpellCrafted?.Invoke(sp);
    }
    public void OnRelicCraftedEffect(Relic r) {
        OnRelicCrafted?.Invoke(r);
    }
    public void OnSpellCasterInitializedEffect(SpellCaster sc){
        OnSpellCasterInitialized?.Invoke(sc);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System;

public class SpellUser : MonoBehaviour
{
    public Hittable hp;
    public HealthBar healthui;
    public ManaBar manaui;

    public SpellCaster spellcaster;
    public SpellUIContainer spelluicontainer;

    public int speed;
    public Unit unit;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        unit = GetComponent<Unit>();
        TrainingRoomEventbus.Instance.player = gameObject;
        GameManager.Instance.player = gameObject;
        EventBus.Instance.OnSpellRemove += DropSpell;
        speed = 10;
    }

    public void SetStats()
    {
        int power = 5;
        spellcaster = new SpellCaster(125, 8, power, Hittable.Team.PLAYER);
        StartCoroutine(spellcaster.ManaRegeneration());
        //TrainingRoomEventbus.Instance.onSpellCasterInitialized(spellcaster);
        hp = new Hittable(100, Hittable.Team.PLAYER, gameObject);
        hp.team = Hittable.Team.PLAYER;

        // tell UI elements what to show
        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);
        for (int i = 0; i < spellcaster.spellCount; i++)
        {
            if (spellcaster.spell[i] != null)
            {
                spelluicontainer.DisplayNewSpell(spellcaster.spell[i], i);
            }
        }
        spelluicontainer.HighlightCurrSpell(spellcaster.selectedSpell);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAttack(InputValue value)
    {
        if (TrainingRoomEventbus.Instance.state == TrainingRoomEventbus.RoomState.MENU) return;
        Vector2 mouseScreen = Mouse.current.position.value;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0;
        //EventBus.Instance.BeforeCat();
        StartCoroutine(spellcaster.Cast(transform.position, mouseWorld));
        //EventBus.Instance.OnCast();
    }
    
    void OnMove(InputValue value)
    {
        if (TrainingRoomEventbus.Instance.state == TrainingRoomEventbus.RoomState.MENU) return;
        unit.movement = value.Get<Vector2>() * this.speed;
    }

    void OnMenu(InputValue value) {
        TrainingRoomEventbus.Instance.OnOpenMenu();
    }
    public void DropSpell(Spell spell, int index)
    {
        spellcaster.DropSpell(index);
        OnChangeSpell();
    }
    void OnChangeSpell()
    {
        spellcaster.nextSpell();
        spelluicontainer.HighlightCurrSpell(spellcaster.selectedSpell);
    }

    
}

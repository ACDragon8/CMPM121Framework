using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour
{
    public Hittable hp;
    public ArrayList relics;
    public HealthBar healthui;
    public ManaBar manaui;

    public SpellCaster spellcaster;
    public SpellUIContainer spelluicontainer;

    public int speed;
    public int baseSpeed;
    public Dictionary<string, int> speedModifiers;
    

    public Unit unit;

    public float lastMoved;

    public bool idleLock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.idleLock = false;
        lastMoved = Time.time;
        relics = new ArrayList();
        unit = GetComponent<Unit>();
        GameManager.Instance.player = gameObject;
        EventBus.Instance.OnSpellRemove += DropSpell;
        this.speedModifiers = new Dictionary<string, int>();
        this.baseSpeed = 10;
    }

    public void StartLevel()
    {
        int power = 5;
        spellcaster = new SpellCaster(125, 8, power, Hittable.Team.PLAYER);
        StartCoroutine(spellcaster.ManaRegeneration());
        
        hp = new Hittable(100, Hittable.Team.PLAYER, gameObject);
        hp.OnDeath += Die;
        hp.OnDeath += GameManager.Instance.OnPlayerDeathEffects;
        hp.team = Hittable.Team.PLAYER;

        //testing relics
        //relics.Add(new Blood(spellcaster));

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
        //check if player has moved in 3 seconds
        if (!this.idleLock)
        {
            if (this.lastMoved + 3.0 <= Time.time)
            {
                this.idleLock = true;
                EventBus.Instance.OnIdle();
            }
        }
    }

    void OnAttack(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        Vector2 mouseScreen = Mouse.current.position.value;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0;
        EventBus.Instance.BeforeCat();
        StartCoroutine(spellcaster.Cast(transform.position, mouseWorld));
        EventBus.Instance.OnCast();
    }
    
    void getSpeed()
    {
        this.speed = this.baseSpeed;
        foreach(var (key,value) in this.speedModifiers)
        {
            this.speed += value;
        }
    }

    public void modifySpeed(string s, int val)
    {
        if (this.speedModifiers.TryGetValue(s, out int a))
        {
            this.speedModifiers[s] = val;
        }
        else
        {
            this.speedModifiers.Add(s, val);
        }
        
    }

    void OnMove(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        getSpeed();
        unit.movement = value.Get<Vector2>() * this.speed;
        lastMoved = Time.time;
        this.idleLock = false;
        EventBus.Instance.OnMove();
    }

    void OnChangeSpell()
    {
        spellcaster.nextSpell();
        spelluicontainer.HighlightCurrSpell(spellcaster.selectedSpell);
    }
    public void DropSpell(Spell spell, int index) {
        spellcaster.DropSpell(index);
        OnChangeSpell();
    }

    public void SetSpeed(int val) {
        this.baseSpeed = val;
    }
    void Die()
    {
        GameManager.Instance.state = GameManager.GameState.GAMEOVER;
        while (relics.Count > 0) {
            Relic r = (Relic) relics[0];
            r.Deactivate();
            relics.Remove(r);
        }
        Debug.Log("You Lost");
    }
    
}

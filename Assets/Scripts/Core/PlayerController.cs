using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Hittable hp;
    public ArrayList relics;
    public HealthBar healthui;
    public ManaBar manaui;

    public SpellCaster spellcaster;
    public SpellUIContainer spelluicontainer;

    public int speed;

    public Unit unit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        relics = new ArrayList();
        unit = GetComponent<Unit>();
        GameManager.Instance.player = gameObject;
        relics.Add(new GreenGem());
        //EventBus.Instance.OnDamage += Test;
    }

    public void StartLevel()
    {
        int power = 5;
        spellcaster = new SpellCaster(125, 8, power, Hittable.Team.PLAYER);
        StartCoroutine(spellcaster.ManaRegeneration());
        
        hp = new Hittable(100, Hittable.Team.PLAYER, gameObject);
        hp.OnDeath += Die;
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
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        Vector2 mouseScreen = Mouse.current.position.value;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0;
        StartCoroutine(spellcaster.Cast(transform.position, mouseWorld));
    }

    void OnMove(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        unit.movement = value.Get<Vector2>()*speed;
    }

    void OnChangeSpell()
    {
        spellcaster.nextSpell();
        spelluicontainer.HighlightCurrSpell(spellcaster.selectedSpell);
    }


    public void SetSpeed(int val) {
        this.speed = val;
    }
    void Die()
    {
        GameManager.Instance.state = GameManager.GameState.GAMEOVER;
        Debug.Log("You Lost");
    }
    
}

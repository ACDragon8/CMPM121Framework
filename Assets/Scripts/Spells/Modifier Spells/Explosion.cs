using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class Explosive : ModifierSpell
{
    public Explosive(SpellCaster owner) : base(owner) { }
    public override void SetProperties(JObject spellAttributes)
    {
        string angl = spellAttributes["damage_multiplier"].ToString();
        if (!float.TryParse(angl, out damage_multiplier))
        {
            Debug.Log("Unable to read angle for Splitter");
        }
        string mana_mult = spellAttributes["mana_adder"].ToString();
        if (!int.TryParse(mana_mult, out mana_adder))
        {
            Debug.Log("Unable to read mana multiplier for Splitter");
        }
        base.SetProperties(spellAttributes);
    }
    public override void ModifySpell()
    {
        baseSpell.SetDamage((int)(baseSpell.GetDamage() * damage_multiplier));
        baseSpell.SetManaCost(baseSpell.GetManaCost() + mana_adder);
        SetOnHitMethod(explosiveOnHit);
    }
    public void explosiveOnHit(Hittable hit, Vector3 where) {
        //Debug.Log("Dealing additional explosion dmg");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(where, 1);
        foreach (Collider2D collider in colliders)
        {
            Hittable target = collider.GetComponent<Hittable>();
            if (target != null && target != hit && target.team != team)
            {
                // Calculate explosion damage
                int explosionDamage = Mathf.RoundToInt((GetDamage()*0.75f));
                target.Damage(new Damage(explosionDamage, Damage.Type.ARCANE));
            }
        }
    }
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        return baseSpell.Cast(where, target, team);
    }

}

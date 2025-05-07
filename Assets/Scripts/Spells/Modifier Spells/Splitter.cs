using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

public class Splitter : ModifierSpell
{
    public Splitter(SpellCaster owner) : base(owner) { }
    public override void SetProperties(JObject spellAttributes)
    {
        string angl = spellAttributes["angle"].ToString();
        if (!int.TryParse(angl, out angle))
        {
            Debug.Log("Unable to read angle for Splitter");
        }
        string mana_mult = spellAttributes["mana_multiplier"].ToString();
        if (!float.TryParse(mana_mult, out mana_multiplier))
        {
            Debug.Log("Unable to read mana multiplier for Splitter");
        }
        base.SetProperties(spellAttributes);
    }
    public override void ModifySpell()
    {
        baseSpell.manaCost = (int)(baseSpell.GetManaCost() * mana_multiplier);
    }
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Vector3 direction = target - where;
        float original_angle = Mathf.Atan2(direction.x, direction.y);
        CoroutineManager.Instance.StartCoroutine(baseSpell.Cast(where, where + new Vector3(Mathf.Sin(original_angle + angle), Mathf.Cos(original_angle + angle), 0), team));
        CoroutineManager.Instance.StartCoroutine(baseSpell.Cast(where, where + new Vector3(Mathf.Sin(original_angle - angle), Mathf.Cos(original_angle - angle), 0), team));
        yield return new WaitForEndOfFrame();
    }
}

using UnityEngine;
using Party = System.Collections.Generic.List<UnityEngine.GameObject>;

public class ExperienceTest : MonoBehaviour, IObserver
{
    private void Start()
    {
        VerifyLevelToExperience();
        VerifySharedExperience();
    }

    private void OnEnable() => AddObservers();
    private void OnDisable() => RemoveObservers();

    private void VerifyLevelToExperience()
    {
        for (var i = 1; i < 100; i++)
        {
            var exp = Rank.ExperienceForLevel(i);
            var lvl = Rank.LevelForExperience(exp);

            if (lvl != i)
                Debug.LogWarning($"Mismatch on level: {i} with exp: {exp} returned: {lvl}");
            else
                Debug.Log($"Level: {lvl} = Exp: {exp}");
        }
    }

    private void VerifySharedExperience()
    {
        var names = new string[] { "Russell", "Brian", "Josh", "Ian", "Adam", "Andy" };
        var heroes = new Party();

        for (var i = 0; i < names.Length; i++)
        {
            var actor = new GameObject(names[i]);
            actor.AddComponent<Stats>();
            var rank = actor.AddComponent<Rank>();
            rank.Init(Random.Range(1, 5));
            heroes.Add(actor);
        }

        Debug.Log("===== Before Adding Experience ======");
        LogParty(heroes);
        Debug.Log("=====================================");
        ExperienceManager.AwardExperience(1000, heroes);
        Debug.Log("===== After Adding Experience ======");
        LogParty(heroes);
    }

    private void LogParty(Party p)
    {
        for (int i = 0; i < p.Count; i++)
        {
            var actor = p[i];
            var rank = actor.GetComponent<Rank>();
            Debug.Log($"Name:{actor.name} Level:{rank.LVL} Exp:{rank.EXP}");
        }
    }

    void OnLevelChange(object sender, object args)
    {
        var stats = sender as Stats;
        Debug.Log(stats.name + " leveled up!");
    }

    void OnExperienceEffect(object sender, object args)
    {
        var actor = (sender as Stats).gameObject;
        var vce = args as ValueChangeEffect;
        int roll = Random.Range(0, 5);
        switch (roll)
        {
            case 0:
                vce.Toggle();
                Debug.Log($"{actor.name} would have received {vce.Delta} experience, but we stopped it");
                break;
            case 1:
                vce.AddModifier(new AddValueModifier(0, 1000));
                Debug.Log($"{actor.name} would have received {vce.Delta} experience, but we added 1000");
                break;
            case 2:
                vce.AddModifier(new MultiplyValueModifier(0, 2f));
                Debug.Log($"{actor.name} would have received {vce.Delta} experience, but we multiplied by 2");
                break;
            default:
                Debug.Log($"{actor.name} will receive {vce.Delta} experience");
                break;
        }
    }

    public void AddObservers()
    {
        this.AddObserver(OnLevelChange, Stats.OnChangeMessage(StatTypes.LVL));
        this.AddObserver(OnExperienceEffect, Stats.BeforeChangeMessage(StatTypes.EXP));
    }

    public void RemoveObservers()
    {
        this.RemoveObserver(OnLevelChange, Stats.OnChangeMessage(StatTypes.LVL));
        this.RemoveObserver(OnExperienceEffect, Stats.BeforeChangeMessage(StatTypes.EXP));
    }

}

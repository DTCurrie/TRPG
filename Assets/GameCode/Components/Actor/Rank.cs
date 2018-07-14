using Unity.Mathematics;
using UnityEngine;

public class Rank : MonoBehaviour, IObserver
{
    private Stats _stats;

    public const int MinLevel = 1;
    public const int MaxLevel = 99;
    public const int MaxExp = 999999;

    public int LVL => _stats[StatTypes.LVL];
    public int EXP
    {
        get => _stats[StatTypes.EXP];
        set => _stats[StatTypes.EXP] = value;
    }

    public float LevelProgress => (LVL - MinLevel) / (float)(MaxLevel - MinLevel);

    private void Awake() => _stats = GetComponent<Stats>();
    private void OnEnable() => AddObservers();
    private void OnDisable() => RemoveObservers();

    private void BeforeExpChange(object sender, object effect) =>
        ((ValueChangeEffect)effect).AddModifier(new ClampValueModifier(int.MaxValue, EXP, MaxExp));

    private void OnExpChange(object sender, object effect) =>
        _stats.SetValue(StatTypes.LVL, LevelForExperience(EXP), false);

    public void Init(int level)
    {
        _stats.SetValue(StatTypes.LVL, level, false);
        _stats.SetValue(StatTypes.EXP, ExperienceForLevel(level), false);
    }

    public void AddObservers()
    {
        this.AddObserver(BeforeExpChange, Stats.BeforeChangeMessage(StatTypes.EXP));
        this.AddObserver(OnExpChange, Stats.OnChangeMessage(StatTypes.EXP));
    }

    public void RemoveObservers()
    {
        this.RemoveObserver(BeforeExpChange, Stats.BeforeChangeMessage(StatTypes.EXP));
        this.RemoveObserver(OnExpChange, Stats.OnChangeMessage(StatTypes.EXP));
    }

    public static int ExperienceForLevel(int level)
    {
        var levelProgress = math.clamp((level - MinLevel) / (float)(MaxLevel - MinLevel), 0, 1);
        return (int)math.clamp(levelProgress * MaxExp, 0, MaxExp);
    }

    public static int LevelForExperience(int exp)
    {
        var level = MaxLevel;
        for (; level >= MinLevel; level--)
            if (exp >= ExperienceForLevel(level))
                break;
        return level;
    }
}

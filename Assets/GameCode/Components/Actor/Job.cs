using Unity.Mathematics;
using UnityEngine;

public class Job : MonoBehaviour
{
    private Stats _stats;

    public static readonly StatTypes[] StatOrder =
    {
        StatTypes.MHP,
        StatTypes.MMP,
        StatTypes.ATK,
        StatTypes.DEF,
        StatTypes.MAT,
        StatTypes.MDF,
        StatTypes.SPD
    };

    public int[] BaseStats = new int[StatOrder.Length];
    public float[] GrowthStats = new float[StatOrder.Length];

    private void OnDestroy()
    {
        this.RemoveObserver(OnLevelChangeMessage, Stats.OnChangeMessage(StatTypes.LVL));
    }

    public void Employ()
    {
        _stats = gameObject.GetComponentInParent<Stats>();
        this.AddObserver(OnLevelChangeMessage, Stats.OnChangeMessage(StatTypes.LVL), _stats);

        var attributes = GetComponentsInChildren<IAttribute>();
        for (var i = 0; i < attributes.Length; i++)
            attributes[i].Activate(transform.parent.gameObject);
    }

    public void Unemploy()
    {
        var attributes = GetComponentsInChildren<IAttribute>();
        for (var i = 0; i < attributes.Length; i++)
            attributes[i].Deactivate();

        this.RemoveObserver(OnLevelChangeMessage, Stats.OnChangeMessage(StatTypes.LVL), _stats);
        _stats = null;
    }

    public void LoadDefaultStats()
    {
        for (var i = 0; i < StatOrder.Length; i++)
        {
            var type = StatOrder[i];
            _stats.SetValue(type, BaseStats[i], false);
        }

        _stats.SetValue(StatTypes.HP, _stats[StatTypes.MHP], false);
        _stats.SetValue(StatTypes.MP, _stats[StatTypes.MMP], false);
    }


    private void OnLevelChangeMessage(object sender, object e)
    {
        var oldLevel = (int)e;
        var newLevel = _stats[StatTypes.LVL];
        for (var i = oldLevel; i < newLevel; ++i)
            LevelUp();
    }

    private void LevelUp()
    {
        for (var i = 0; i < StatOrder.Length; i++)
        {
            var type = StatOrder[i];
            var whole = (int)math.floor(GrowthStats[i]);
            var fraction = GrowthStats[i] - whole;

            var value = _stats[type];
            value += whole;

            if (Random.value > (1f - fraction)) value++;

            _stats.SetValue(StatTypes.HP, _stats[StatTypes.MHP], false);
            _stats.SetValue(StatTypes.MP, _stats[StatTypes.MMP], false);
        }
    }
}

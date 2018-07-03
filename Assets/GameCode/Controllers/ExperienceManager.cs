using Unity.Mathematics;
using System.Collections.Generic;
using Party = System.Collections.Generic.List<UnityEngine.GameObject>;

public static class ExperienceManager
{
    private const float _minLevelBonus = 1.5f;
    private const float _maxLevelBonus = 0.5f;

    public static void AwardExperience(int amount, Party party)
    {
        var ranks = new List<Rank>(party.Count);

        for (var i = 0; i < party.Count; i++)
        {
            var rank = party[i].GetComponent<Rank>();
            if (rank != null) ranks.Add(rank);
        }

        var min = int.MaxValue;
        var max = int.MinValue;

        for (var i = 0; i < ranks.Count; i++)
        {
            min = math.min(ranks[i].LVL, min);
            max = math.max(ranks[i].LVL, max);
        }

        var weights = new float[party.Count];
        var weightTotal = 0f;

        for (var i = 0; i < ranks.Count; i++)
        {
            var percent = (ranks[i].LVL - min) / (float)(max - min);
            weights[i] = math.lerp(_minLevelBonus, _maxLevelBonus, percent);
            weightTotal += weights[i];
        }

        for (var i = 0; i < ranks.Count; i++)
            ranks[i].EXP += (int)math.floor((weights[i] / weightTotal) * amount);
    }
}

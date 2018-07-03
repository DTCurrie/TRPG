using System;
using UnityEngine;

public class JobData : ScriptableObject, IParseable
{
    public string Name { get; private set; }

    public int[] BaseStats = new int[Job.StatOrder.Length];
    public float[] GrowthStats = new float[Job.StatOrder.Length];

    public int MOV;
    public int JMP;

    public void Load(string line)
    {
        var elements = line.Split(',');

        Name = elements[0];

        for (var i = 0; i < Job.StatOrder.Length * 2; i += 2)
        {
            BaseStats[i / 2] = Convert.ToInt32(elements[i + 1]);
            GrowthStats[i / 2] = float.Parse(elements[i + 2]);
        }

        MOV = Convert.ToInt32(elements[15]);
        JMP = Convert.ToInt32(elements[16]);
    }
}

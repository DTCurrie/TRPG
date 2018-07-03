using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolData
{
    public GameObject Prefab;
    public int MaxCount;
    public Queue<Poolable> Poolables;

    public PoolData(GameObject prefab, int maxCount, int prepopulate)
    {
        Prefab = prefab;
        MaxCount = maxCount;
        Poolables = new Queue<Poolable>(prepopulate);
    }
}
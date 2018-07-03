using UnityEngine;
using System.Collections.Generic;

public class PoolController : MonoBehaviour
{
    private static int _index;
    private static Dictionary<string, int> _indexes = new Dictionary<string, int>();

    public readonly static PoolController Instance = CreateInstance();
    public static Dictionary<int, PoolData> Pools = new Dictionary<int, PoolData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
    }

    private static Poolable CreatePoolable(int index, GameObject prefab)
    {
        var poolable = Instantiate(prefab).AddComponent<Poolable>();
        poolable.Index = index;
        return poolable;
    }

    private static PoolController CreateInstance()
    {
        var obj = new GameObject("Pool Controller");
        DontDestroyOnLoad(obj);
        return obj.AddComponent<PoolController>();
    }

    public static int GetIndex(string key)
    {
        if (!_indexes.ContainsKey(key)) _indexes.Add(key, _index++);
        return _indexes[key];
    }

    public static bool AddPool(int index, GameObject prefab, int maxCount, int prepopulate)
    {
        if (Pools.ContainsKey(index)) return false;

        var pool = new PoolData(prefab, maxCount, prepopulate);
        Pools.Add(index, pool);

        for (int i = 0; i < prepopulate; i++)
            Enqueue(CreatePoolable(index, prefab));

        return true;
    }

    public static bool ClearPool(int index)
    {
        if (!Pools.ContainsKey(index)) return false;

        var pool = Pools[index];

        while (pool.Poolables.Count > 0)
            Destroy(pool.Poolables.Dequeue().gameObject);

        return Pools.Remove(index);
    }

    public static bool Enqueue(Poolable poolable)
    {
        if (poolable == null || poolable.Pooled || !Pools.ContainsKey(poolable.Index))
            return false;

        var pool = Pools[poolable.Index];
        if (pool.Poolables.Count >= pool.MaxCount)
        {
            Destroy(poolable.gameObject);
            return false;
        }

        pool.Poolables.Enqueue(poolable);
        poolable.Pooled = true;
        poolable.gameObject.SetActive(false);
        return true;
    }

    public static Poolable Dequeue(int index)
    {
        if (!Pools.ContainsKey(index)) return null;

        var pool = Pools[index];
        if (pool.Poolables.Count == 0) return CreatePoolable(index, pool.Prefab);

        var poolable = pool.Poolables.Dequeue();
        poolable.Pooled = false;
        return poolable;
    }
}
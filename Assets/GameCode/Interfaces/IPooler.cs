using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooler<T> where T : MonoBehaviour
{
    //string PoolKey { get; }
    //int PoolIndex { get; }
    //GameObject Prefab { get; }
    //List<Poolable> Instances { get; }

    T Dequeue();
    void Enqueue(T obj);
}

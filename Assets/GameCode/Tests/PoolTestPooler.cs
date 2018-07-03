using System.Collections.Generic;
using UnityEngine;

public class PoolTestPooler : MonoBehaviour, IPooler<MonoBehaviour>
{
    public int Index;
    public string Label;
    public GameObject Prefab;
    public List<Poolable> Instances = new List<Poolable>();

    private int _left => 10 + (Index * 110);

    private void Start()
    {
        Index = PoolController.GetIndex(Label);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(_left, 90, 100, 30), Label);

        if (GUI.Button(new Rect(_left, 130, 100, 30), "Dequeue")) Dequeue();
        if (GUI.Button(new Rect(_left, 170, 100, 30), "Enqueue"))
        {
            if (Instances.Count > 0)
            {
                var obj = Instances[0];
                Enqueue(obj);
            }
        }
    }

    public MonoBehaviour Dequeue()
    {
        var obj = PoolController.Dequeue(Index);
        obj.transform.localPosition = new Vector3(
            Random.Range(-10, 10),
            Random.Range(0, 5),
            Random.Range(0, 10));
        obj.gameObject.SetActive(true);
        Instances.Add(obj);
        return obj;
    }

    public void Enqueue(MonoBehaviour obj)
    {
        Instances.RemoveAt(0);
        PoolController.Enqueue(obj.GetComponent<Poolable>());
    }
}

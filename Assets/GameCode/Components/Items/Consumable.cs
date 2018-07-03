using UnityEngine;
using System.Collections;

public class Consumable : MonoBehaviour
{
    public void Consume(GameObject target)
    {
        var attributes = GetComponentsInChildren<IAttribute>();
        for (var i = 0; i < attributes.Length; i++) 
            attributes[i].Activate(target, true);
    }
}

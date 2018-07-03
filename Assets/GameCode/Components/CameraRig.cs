using UnityEngine;
using System.Collections;

public class CameraRig : MonoBehaviour
{
    private Transform _transform;

    public float Speed = 3f;
    public Transform Follow;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (Follow) 
            _transform.position = Vector3.Lerp(_transform.position, Follow.position, Speed * Time.deltaTime);
    }
}

using UnityEngine;
using System.Collections;

public class CameraRig : MonoBehaviour
{
    private Transform _transform;

    public float MoveTime = 0.15f;
    public Transform Follow;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (Follow) StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        var position = _transform.position;
        var elapsed = 0f;

        while (elapsed <= MoveTime)
        {
            elapsed += Time.deltaTime;
            _transform.position = Vector3.Lerp(position, Follow.position, elapsed / MoveTime);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}

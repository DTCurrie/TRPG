using UnityEngine;
using System.Collections;

public class TeleportMovement : MonoBehaviour, IMovement
{
    public Unit Unit { get; private set; }

    public int Range => 5;

    private void Awake() => Unit = GetComponent<Unit>();

    public bool ExpandSearch(Tile from, Tile to) => this.SimpleSearch(from);

    public IEnumerator Move(Tile target)
    {
        var direction = Unit.CurrentTile.GetDirection(target);
        var rotation = Quaternion.Euler(direction.ToEuler() + transform.rotation.eulerAngles);

        float angle;
        Vector3 axis;
        rotation.ToAngleAxis(out angle, out axis);

        var targetAngle = angle + 1080f;

        var teleportTime = 3f;
        var scale = transform.localScale;
        var elapsed = 0f;

        Unit.Place(target);

        while (elapsed <= teleportTime)
        {
            elapsed += Time.deltaTime;
            var currentAngle = Mathf.Lerp(angle, targetAngle, elapsed / teleportTime);
            transform.rotation = Quaternion.AngleAxis(currentAngle, axis);
            transform.localScale = Vector3.Lerp(scale, Vector3.zero, elapsed / teleportTime);
            yield return new WaitForEndOfFrame();
        }

        transform.position = target.CenterTop;
        elapsed = 0f;

        while (elapsed <= teleportTime)
        {
            elapsed += Time.deltaTime;
            var currentAngle = Mathf.Lerp(angle, targetAngle, elapsed / teleportTime);
            transform.rotation = Quaternion.AngleAxis(currentAngle, axis);
            transform.localScale = Vector3.Lerp(Vector3.zero, scale, elapsed / teleportTime);
            yield return new WaitForEndOfFrame();
        }

        transform.localScale = Vector3.one;
        yield return StartCoroutine(this.Turn(direction));
        yield return null;
    }
}

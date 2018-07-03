using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkMovement : MonoBehaviour, IMovement
{
    private Stats _stats;

    public Unit Unit { get; private set; }

    public int Range => 5;
    public int JumpHeight = 3;

    private void Awake() => Unit = GetComponent<Unit>();
    private void Start() => _stats = GetComponent<Stats>();

    private IEnumerator Walk(Tile to)
    {
        var position = transform.position;
        var targetPosition = to.CenterTop;
        var speed = 3f;
        var time = Vector3.Distance(position, targetPosition) / speed;
        var threshold = 0.05f;
        var elapsed = 0f;

        while (Vector3.Distance(transform.position, targetPosition) >= threshold)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(position, targetPosition, elapsed / time);
            yield return new WaitForEndOfFrame();
        }

        transform.position = targetPosition;
        yield return null;
    }

    private IEnumerator Jump(Tile to)
    {
        var position = transform.position;
        var targetPosition = to.CenterTop;
        var speed = 3f;
        var time = Vector3.Distance(position, targetPosition) / speed;
        var jumpTop = (Settings.StepHeight * 2) + transform.position.y < targetPosition.y ? targetPosition.y : transform.position.y;
        var jumped = false;
        var threshold = 0.05f;
        var elapsed = 0f;

        while (Vector3.Distance(transform.position, targetPosition) >= threshold)
        {
            elapsed += Time.deltaTime;
            var jumping = jumped ? false : transform.position.y <= jumpTop - threshold;

            transform.position = Vector3.Lerp(position, new Vector3(targetPosition.x, transform.position.y, targetPosition.z), elapsed / time);
            transform.position = Vector3.Lerp(position, new Vector3(transform.position.x, jumping ? jumpTop : targetPosition.y, transform.position.z), elapsed / time);

            jumped |= !jumping;
            yield return new WaitForEndOfFrame();
        }

        transform.position = targetPosition;
        yield return null;
    }

    public bool ExpandSearch(Tile from, Tile to)
    {
        if ((Mathf.Abs(from.Height - to.Height) > JumpHeight)) return false;
        if (to.Content != null) return false;
        return this.SimpleSearch(from);
    }

    public IEnumerator Move(Tile target)
    {
        var targetTiles = new List<Tile>();

        Unit.Place(target);

        while (target != null)
        {
            targetTiles.Insert(0, target);
            target = target.PathFindingPrevious;
        }

        for (var i = 1; i < targetTiles.Count; i++)
        {
            var from = targetTiles[i - 1];
            var to = targetTiles[i];
            var direction = from.GetDirection(to);


            if (Unit.Direction != direction)
                yield return StartCoroutine(this.Turn(direction));


            yield return StartCoroutine(from.Height == to.Height ? Walk(to) : Jump(to));
        }

        yield return null;
    }
}

using System.Collections;
using System.Linq;
using UnityEngine;

public class FlyMovement : MonoBehaviour, IMovement
{
    private BattleController _controller;

    public Unit Unit { get; private set; }

    public int Range => 5;

    private void Awake()
    {
        Unit = GetComponent<Unit>();
        _controller = GameObject.FindGameObjectsWithTag("GameController")
                                .First(c => c.GetComponent<BattleController>() != null)
                                .GetComponent<BattleController>();
    }

    public bool ExpandSearch(Tile from, Tile to) => this.SimpleSearch(from);

    public IEnumerator Move(Tile target)
    {

        var direction = Unit.CurrentTile.GetDirection(target);
        var targetPosition = target.CenterTop;
        var targetY = _controller.Board.Tiles.Values.Aggregate((t1, t2) => t1.Height > t2.Height ? t1 : t2).CalculatedHeight + 2;
        var floatingPosition = new Vector3(transform.position.x, targetY, transform.position.z);
        var targetFloatingPosition = new Vector3(targetPosition.x, targetY, targetPosition.z);
        var speed = 3f;
        var time = Vector3.Distance(floatingPosition, targetFloatingPosition) / speed;
        var elapsed = 0f;
        var threshold = 0.05f;

        Unit.Place(target);
       StartCoroutine(this.Turn(direction));

        while (Vector3.Distance(transform.position, floatingPosition) >= threshold)
        {
            transform.position = Vector3.Lerp(transform.position, floatingPosition, Time.deltaTime * speed);
            yield return new WaitForEndOfFrame();
        }

        transform.position = floatingPosition;

        while (Vector3.Distance(transform.position, targetFloatingPosition) >= threshold)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(floatingPosition, targetFloatingPosition, elapsed / time);
            yield return new WaitForEndOfFrame();
        }

        transform.position = targetFloatingPosition;

        while (Vector3.Distance(transform.position, targetPosition) >= threshold)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
            yield return new WaitForEndOfFrame();
        }

        transform.position = targetPosition;
        yield return null;
    }
}

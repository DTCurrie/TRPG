using Unity.Mathematics;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Renderer _renderer;

    private Color _defaultColor;
    [SerializeField] private Color _selectedColor = new Color(0, 1, 1, 1);

    public float2 Coordinates;
    public int Height;
    public GameObject Content;

    public Tile PathFindingPrevious { get; set; }
    public int PathfindingCost { get; set; }

    public float CalculatedHeight => Height * Settings.StepHeight;
    public float3 CenterTop => new float3(Coordinates.x, CalculatedHeight, Coordinates.y);

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _defaultColor = _renderer.material.color;
    }

    private void Refresh()
    {
        transform.localPosition = new float3(Coordinates.x, CalculatedHeight / 2f, Coordinates.y);
        transform.localScale = new float3(1, CalculatedHeight, 1);
    }

    public void Adjust(int height)
    {
        Height += height;
        Refresh();
    }

    public void Place(float2 coordinates, int height)
    {
        Coordinates = new float2((int)coordinates.x, (int)coordinates.y);
        Height = height;
        Refresh();
    }

    public void Place(float3 data) => Place(new float2((int)data.x, (int)data.z), (int)data.y);

    public void Select(bool selecting) => _renderer.material.SetColor("_Color", selecting ? _selectedColor : _defaultColor);

    public void ResetPathfindingData()
    {
        PathFindingPrevious = null;
        PathfindingCost = int.MaxValue;
    }
}

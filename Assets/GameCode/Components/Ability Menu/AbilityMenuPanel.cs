using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class AbilityMenuPanel : MonoBehaviour
{
    public RectTransform PanelRect;

    private void Start()
    {
        if (!PanelRect) PanelRect = GetComponent<RectTransform>();
    }
}

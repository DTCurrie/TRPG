using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class StatPanelController : MonoBehaviour
{
    private Vector2 _primaryPanelPosition;
    private Vector2 _secondaryPanelPosition;

    public StatPanel PrimaryPanel;
    public StatPanel SecondaryPanel;

    private float2 PanelHiddenPosition(StatPanel panel) =>
        new float2(-panel.PanelRect.anchoredPosition.x, panel.PanelRect.anchoredPosition.y);

    private void Start()
    {
        _primaryPanelPosition = PrimaryPanel.PanelRect.anchoredPosition;
        _secondaryPanelPosition = SecondaryPanel.PanelRect.anchoredPosition;

        PrimaryPanel.PanelRect.anchoredPosition = PanelHiddenPosition(PrimaryPanel);
        SecondaryPanel.PanelRect.anchoredPosition = PanelHiddenPosition(SecondaryPanel);
    }

    public void ShowPrimary(GameObject obj)
    {
        PrimaryPanel.Display(obj);
        if (PrimaryPanel.PanelRect.anchoredPosition == _primaryPanelPosition)
            return;
        StartCoroutine(MovePanel(PrimaryPanel, _primaryPanelPosition));
    }

    public void HidePrimary()
    {
        if (PrimaryPanel.PanelRect.anchoredPosition != _primaryPanelPosition) return;
        StartCoroutine(MovePanel(PrimaryPanel, PanelHiddenPosition(PrimaryPanel)));
    }

    public void ShowSecondary(GameObject obj)
    {
        if (SecondaryPanel.PanelRect.anchoredPosition == _secondaryPanelPosition)
            return;
        SecondaryPanel.Display(obj);
        StartCoroutine(MovePanel(SecondaryPanel, _secondaryPanelPosition));
    }

    public void HideSecondary()
    {
        if (PrimaryPanel.PanelRect.anchoredPosition != _secondaryPanelPosition) return;
        StartCoroutine(MovePanel(SecondaryPanel, PanelHiddenPosition(SecondaryPanel)));
    }

    private IEnumerator MovePanel(StatPanel panel, Vector2 targetPosition)
    {
        var position = panel.PanelRect.anchoredPosition;
        var time = 0.5f;
        var threshold = 0.05f;
        var elapsed = 0f;

        while (Vector3.Distance(panel.PanelRect.anchoredPosition, targetPosition) >= threshold)
        {
            elapsed += Time.deltaTime;
            panel.PanelRect.anchoredPosition = Vector2.Lerp(position, targetPosition, elapsed / time);
            yield return new WaitForEndOfFrame();
        }

        panel.PanelRect.anchoredPosition = targetPosition;
        yield return null;
    }

}

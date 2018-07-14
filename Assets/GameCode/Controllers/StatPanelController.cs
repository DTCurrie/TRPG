using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class StatPanelController : MonoBehaviour
{
    private Vector2 _primaryPanelPosition;
    private Vector2 _secondaryPanelPosition;

    private bool _showingPrimary;
    private bool _showingSecondary;

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

        _showingPrimary = false;
        _showingSecondary = false;
    }

    public void ShowPrimary(GameObject obj)
    {
        PrimaryPanel.Display(obj);
        if (_showingPrimary) return;
        StartCoroutine(MovePanel(PrimaryPanel, _primaryPanelPosition));
        _showingPrimary = true;
    }

    public void HidePrimary()
    {
        if (!_showingPrimary) return;
        StartCoroutine(MovePanel(PrimaryPanel, PanelHiddenPosition(PrimaryPanel)));
        _showingPrimary = false;
    }

    public void ShowSecondary(GameObject obj)
    {
        SecondaryPanel.Display(obj);
        if (_showingSecondary) return;
        StartCoroutine(MovePanel(SecondaryPanel, _secondaryPanelPosition));
        _showingSecondary = true;
    }

    public void HideSecondary()
    {
        if (!_showingSecondary) return;
        StartCoroutine(MovePanel(SecondaryPanel, PanelHiddenPosition(SecondaryPanel)));
        _showingSecondary = false;
    }

    private IEnumerator MovePanel(StatPanel panel, Vector2 targetPosition)
    {
        var position = panel.PanelRect.anchoredPosition;
        var time = 0.5f;
        var elapsed = 0f;

        while (elapsed <= time)
        {
            elapsed += Time.deltaTime;
            panel.PanelRect.anchoredPosition = Vector2.Lerp(position, targetPosition, elapsed / time);
            yield return new WaitForEndOfFrame();
        }

        panel.PanelRect.anchoredPosition = targetPosition;
        yield return null;
    }

}

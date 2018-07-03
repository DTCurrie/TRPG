using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class ConversationController : MonoBehaviour
{
    private Vector2 _leftPanelPosition;
    private Vector2 _rightPanelPosition;

    public Canvas Canvas;

    public ConversationPanel LeftPanel;
    public ConversationPanel RightPanel;

    public IEnumerator Conversation;

    public static event EventHandler CompleteEvent;

    private float2 PanelHiddenPosition(ConversationPanel panel) =>
        new float2(-panel.PanelRect.anchoredPosition.x, panel.PanelRect.anchoredPosition.y);

    private void Start()
    {
        Canvas = GetComponentInChildren<Canvas>();

        _leftPanelPosition = LeftPanel.PanelRect.anchoredPosition;
        _rightPanelPosition = RightPanel.PanelRect.anchoredPosition;

        LeftPanel.PanelRect.anchoredPosition = PanelHiddenPosition(LeftPanel);
        RightPanel.PanelRect.anchoredPosition = PanelHiddenPosition(RightPanel);

        Canvas.gameObject.SetActive(false);
    }

    private IEnumerator Sequence(ConversationData conversation)
    {
        for (var i = 0; i < conversation.SpeechList.Count; i++)
        {
            var speech = conversation.SpeechList[i];
            var currentPanel = (int)speech.TextPosition.x == 0 ? LeftPanel : RightPanel;
            var presentation = currentPanel.Display(speech);

            presentation.MoveNext();

            var hidePosition = PanelHiddenPosition((int)speech.TextPosition.x == 0 ? LeftPanel : RightPanel);

            StartCoroutine(MovePanel(currentPanel, currentPanel == LeftPanel ? _leftPanelPosition : _rightPanelPosition));
            yield return null;

            while (presentation.MoveNext()) yield return null;

            StartCoroutine(MovePanel(currentPanel, hidePosition));
            CompleteEvent += delegate { Conversation.MoveNext(); };
        }

        Canvas.gameObject.SetActive(false);
        CompleteEvent?.Invoke(this, EventArgs.Empty);
        yield return null;
    }

    private IEnumerator MovePanel(ConversationPanel panel, Vector2 targetPosition)
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

    public void Show(ConversationData conversation)
    {
        Canvas.gameObject.SetActive(true);
        Conversation = Sequence(conversation);
        Conversation.MoveNext();
    }

    public void Next()
    {
        if (Conversation == null) return;
        Conversation.MoveNext();
    }
}

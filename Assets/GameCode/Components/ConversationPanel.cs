using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class ConversationPanel : MonoBehaviour
{
    [SerializeField] private int _bounceRange = 5;

    public RectTransform PanelRect;
    public TextMeshProUGUI Message;
    public Image Speaker;
    public GameObject Arrow;

    private void Start()
    {
        if (!PanelRect) PanelRect = GetComponent<RectTransform>();
        if (!Message) Message = GetComponentInChildren<TextMeshProUGUI>();
        if (!Speaker) Speaker = GetComponentInChildren<Image>();
        StartCoroutine(ArrowBounce());
    }

    private Vector2 BounceVector(bool up = true) => (up ? Vector2.up : Vector2.down) * _bounceRange;

    private IEnumerator ArrowBounce()
    {
        var rect = Arrow.GetComponent<RectTransform>();
        var position = rect.anchoredPosition;
        var top = position + BounceVector();
        var bottom = position + BounceVector(false);
        var time = 0.5f;
        var elapsed = 0f;

        rect.anchoredPosition = top;
        var target = bottom;
        var origin = top;

        while (true)
        {
            if (rect.anchoredPosition.y >= position.y + _bounceRange)
            {
                target = bottom;
                origin = top;
                elapsed = 0f;
            }

            if (rect.anchoredPosition.y <= position.y - _bounceRange)
            {
                target = top;
                origin = bottom;
                elapsed = 0f;
            }

            elapsed += Time.deltaTime;
            rect.anchoredPosition = Vector2.Lerp(origin, target, elapsed / time);
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator Display(SpeechData speechData)
    {
        Speaker.sprite = speechData.SpeakerPortrait;
        Speaker.SetNativeSize();

        for (int i = 0; i < speechData.Messages.Count; i++)
        {
            Message.text = speechData.Messages[i];
            Arrow.SetActive(i + 1 < speechData.Messages.Count);
            yield return null;
        }
    }
}

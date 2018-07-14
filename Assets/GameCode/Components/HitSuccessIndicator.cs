using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HitSuccessIndicator : MonoBehaviour
{
    private Vector2 _menuPosition;
    [SerializeField] private Vector2 _menuHiddenPosition;

    public Canvas Canvas;
    public RectTransform Panel;
    public Image Bar;
    public TextMeshProUGUI Label;

    private void Start()
    {
        _menuPosition = Panel.anchoredPosition;
        _menuHiddenPosition = new float2(_menuPosition.x, -_menuPosition.y);

        Panel.anchoredPosition = _menuHiddenPosition;
        Canvas.gameObject.SetActive(false);
    }

    private IEnumerator MovePanel(Vector2 targetPosition)
    {
        var position = Panel.anchoredPosition;
        var time = 0.5f;
        var elapsed = 0f;

        while (elapsed <= time)
        {
            elapsed += Time.deltaTime;
            Panel.anchoredPosition = Vector2.Lerp(position, targetPosition, elapsed / time);
            yield return new WaitForEndOfFrame();
        }

        Panel.anchoredPosition = targetPosition;
        yield return null;
    }

    public void SetStats(int chance, int amount)
    {
        Bar.fillAmount = chance / 100f;
        Label.text = $"{chance}% {amount}pt(s)";
    }

    public IEnumerator Show()
    {
        Canvas.gameObject.SetActive(true);
        yield return StartCoroutine(MovePanel(_menuPosition));
    }

    public IEnumerator Hide()
    {
        yield return StartCoroutine(MovePanel(_menuHiddenPosition));
        Canvas.gameObject.SetActive(false);
        yield return null;
    }

}

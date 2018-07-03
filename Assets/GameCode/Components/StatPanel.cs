using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class StatPanel : MonoBehaviour
{
    public RectTransform PanelRect;

    public Sprite AllyBackground;
    public Sprite EnemyBackground;

    public Image Background;
    public Image Avatar;

    public TextMeshProUGUI NameLabel;
    public TextMeshProUGUI HpLabel;
    public TextMeshProUGUI MpLabel;
    public TextMeshProUGUI LvlLabel;

    public void Display(GameObject obj)
    {
        var stats = obj.GetComponent<Stats>();

        Background.sprite = Random.value > 0.5f ? EnemyBackground : AllyBackground;
        NameLabel.text = obj.name;

        if (stats)
        {
            HpLabel.text = $"HP {stats[StatTypes.HP]} / {stats[StatTypes.MHP]}";
            MpLabel.text = $"MP {stats[StatTypes.MP]} / {stats[StatTypes.MMP]}";
            LvlLabel.text = $"LVL {stats[StatTypes.LVL]}";
        }
    }
}

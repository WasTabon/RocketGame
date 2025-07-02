using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "RocketData", menuName = "RocketPlay/MisionData", order = 0)]
public class MissionData : ScriptableObject
{
    [Header("Основные параметры")] public string missionName;
    [TextArea] public string description;
    [TextArea] public string place;
    [TextArea] public string goodDeliver;
    [TextArea] public string badDeliver;

    [Header("Особенность")] public string condiotion;

    [Header("Визуал")] public Sprite icon;
    
    public RectTransform timerBackground;
    public TextMeshProUGUI timerText;
}

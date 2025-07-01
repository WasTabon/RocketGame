using UnityEngine;

[CreateAssetMenu(fileName = "RocketData", menuName = "RocketPlay/MisionData", order = 0)]
public class MissionData : ScriptableObject
{
    [Header("Основные параметры")] public string missionName;
    [TextArea] public string description;

    [Header("Особенность")] public string condiotion;

    [Header("Визуал")] public Sprite icon;
}

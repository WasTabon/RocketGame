using UnityEngine;

[CreateAssetMenu(fileName = "RocketData", menuName = "RocketPlay/RocketData", order = 0)]
public class RocketData : ScriptableObject
{
    [Header("Основные параметры")] public string rocketName;
    [TextArea] public string description;

    [Header("Характеристики (0–5)")] [Range(0, 5)]
    public int speed;

    [Range(0, 5)] public int fuel;
    [Range(0, 5)] public int cargoProtection;

    [Header("Особенность")] public string specialAbility;

    [Header("Визуал")] public Sprite icon;

    public RectTransform locked;

    public GameObject rocketPrefab;
    public GameObject platform;
}

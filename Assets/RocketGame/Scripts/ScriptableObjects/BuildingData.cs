using UnityEngine;

public enum BuildingType
{
    Static,
    RocketHub,
    Garage,
    Laboratory
}

[CreateAssetMenu(fileName = "New BuildingData", menuName = "Game/Building Data")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    [TextArea(5, 10)]
    public string buildingInfo;

    public BuildingType buildingType;
}
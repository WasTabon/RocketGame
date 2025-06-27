using UnityEngine;

[CreateAssetMenu(fileName = "New BuildingData", menuName = "Game/Building Data")]
public class BuildingData : ScriptableObject
{
    [TextArea(5, 10)]
    public string buildingInfo;
}
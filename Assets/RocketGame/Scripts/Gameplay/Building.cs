using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private BuildingData _buildingData;
    
    public virtual void Click()
    {
        UIController.Instance.ShowInfoButton(_buildingData);
    }
}

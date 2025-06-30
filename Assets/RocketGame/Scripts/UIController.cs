using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private TextMeshProUGUI _infoText;
    
    [SerializeField] private RectTransform _infoButton;
    [SerializeField] private RectTransform _infoPanel;
    [SerializeField] private RectTransform _rocketHubPanel;

    private BuildingData _currentBuildingData;
    
    private void Awake()
    {
        Instance = this;
    }
    
    public void ShowInfoButton(BuildingData data)
    {
        _currentBuildingData = data;
        _infoButton.gameObject.SetActive(true);
    }

    private void SetupInfoPanel(BuildingData data)
    {
        _infoPanel.gameObject.SetActive(true);
        if (data.buildingType == BuildingType.RocketHub)
            _rocketHubPanel.gameObject.SetActive(true);
        if (data.buildingType == BuildingType.RocketHub)
            _rocketHubPanel.gameObject.SetActive(true);
    }
}

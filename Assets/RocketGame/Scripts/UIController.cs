using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [Header("Rocket Info Panel")]
    [SerializeField] private GameObject rocketInfoPanel;
    [SerializeField] private TextMeshProUGUI rocketNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI fuelText;
    [SerializeField] private TextMeshProUGUI cargoProtectionText;
    [SerializeField] private TextMeshProUGUI specialAbilityText;
    [SerializeField] private Image iconImage;
    
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

    
    public void ShowRocketInfo(RocketData rocketData)
    {
        rocketInfoPanel.SetActive(true);

        rocketNameText.text = rocketData.rocketName;
        descriptionText.text = rocketData.description;

        speedText.text = $"Speed: {rocketData.speed}/5";
        fuelText.text = $"Fuel: {rocketData.fuel}/5";
        cargoProtectionText.text = $"Protection: {rocketData.cargoProtection}/5";

        specialAbilityText.text = $"Special: {rocketData.specialAbility}";
        iconImage.sprite = rocketData.icon;
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

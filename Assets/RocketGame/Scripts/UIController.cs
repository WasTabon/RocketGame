using System;
using System.Collections.Generic;
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
    [SerializeField] private Image speedIconFill;
    [SerializeField] private Image fuelIconFill;
    [SerializeField] private Image protectionIconFill;
    
    [Header("Other Panels")]
    [SerializeField] private TextMeshProUGUI _infoText;
    
    [SerializeField] private RectTransform _infoButton;
    [SerializeField] private RectTransform _infoPanel;
    [SerializeField] private RectTransform _rocketHubPanel;
    [SerializeField] private RectTransform _nextButton;

    private BuildingData _currentBuildingData;

    private List<RectTransform> _panels;
    private int _currentPanel;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _panels = new List<RectTransform>();
    }

    public void ShowInfoButton(BuildingData data)
    {
        _currentBuildingData = data;
        _infoButton.gameObject.SetActive(true);
    }

    public void MoveToNextPanel()
    {
        if (_panels[_currentPanel + 1] != null)
        {
            _panels[_currentPanel].gameObject.SetActive(false);
            _panels[_currentPanel + 1].gameObject.SetActive(true);
            _currentPanel++;
        }
    }
    public void MoveToPreviousPanel()
    {
        if (_panels[_currentPanel - 1] != null)
        {
            _panels[_currentPanel].gameObject.SetActive(false);
            _panels[_currentPanel - 1].gameObject.SetActive(true);
            _currentPanel--;
        }
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
        
        speedIconFill.fillAmount = rocketData.speed / 5f;
        fuelIconFill.fillAmount = rocketData.fuel / 5f;
        protectionIconFill.fillAmount = rocketData.cargoProtection / 5f;
    }
    
    private void SetupInfoPanel(BuildingData data)
    {
        _infoPanel.gameObject.SetActive(true);
        if (data.buildingType == BuildingType.RocketHub)
        {
            _panels.Clear();
            _currentPanel = 0;
            _panels.Add(_rocketHubPanel);
        }

        if (_panels.Count <= 0)
        {
            _nextButton.gameObject.SetActive(false);
        }
        else
        {
            _nextButton.gameObject.SetActive(false);
        }
    }
}

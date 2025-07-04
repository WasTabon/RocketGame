using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    
    [SerializeField] private GameObject _avialibleRocketsPanel;
    
    [Header("Cards")] 
    [SerializeField] private RectTransform _avialibleRocketCard;
    [SerializeField] private RectTransform _avialibleRocketContent;
    
    [Header("Mission Info Panel")]
    [SerializeField] private GameObject missionInfoPanel;
    [SerializeField] private TextMeshProUGUI missionNameText;
    [SerializeField] private TextMeshProUGUI missionDescriptionText;
    [SerializeField] private TextMeshProUGUI missionPlaceText;
    [SerializeField] private TextMeshProUGUI missionConditionText;
    [SerializeField] private Image missionIconImage;
    
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
    [SerializeField] private TextMeshProUGUI _infoName;
    
    [SerializeField] private RectTransform _infoButton;
    [SerializeField] private RectTransform _infoPanel;
    [SerializeField] private RectTransform _rocketHubPanel;
    [SerializeField] private RectTransform _nextButton;

    private BuildingData _currentBuildingData;
    private MissionData _currentMissionData;

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
        SetupInfoPanel(data);
        _infoButton.gameObject.SetActive(true);
    }

    public void MoveToNextPanel()
    {
        //_panels[0].gameObject.SetActive(false);
        _panels[1].gameObject.SetActive(true);
        _currentPanel++;
    }
    public void MoveToPreviousPanel()
    {
        _panels[1].gameObject.SetActive(false);
        //_panels[0].gameObject.SetActive(true);
        _currentPanel--;
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

    public void ShowMissionInfo(MissionData missionData)
    {
        _currentMissionData = missionData;
        
        missionInfoPanel.SetActive(true);

        missionNameText.text = missionData.missionName;
        missionDescriptionText.text = missionData.description;
        missionPlaceText.text = missionData.place;
        missionConditionText.text = $"Condition: {missionData.condiotion}";
        missionIconImage.sprite = missionData.icon;
    }

    public void ShowAcceptMission()
    {
        List<RocketState> rockets = RocketHubController.Instance.GetAvialibleRockets();

        foreach (RocketState rocketState in rockets)
        {
            GameObject card = Instantiate(_avialibleRocketCard.gameObject, _avialibleRocketContent);
            TextMeshProUGUI cardName = card.GetComponentInChildren<TextMeshProUGUI>();
            cardName.text = rocketState.rocketData.rocketName;
            card.GetComponentInChildren<Button>().onClick.AddListener(() => AcceptMission(rocketState));
            Debug.Log($"Mission added tp button {card.gameObject}");
        }
    }

    public void HideAcceptMission()
    {
        foreach (Transform child in _avialibleRocketContent)
        {
            Destroy(child.gameObject);
        }
    }

   public void AcceptMission(RocketState state)
{
    Debug.Log("Mission Accepted");
    
    RocketHubController.Instance.AssignMissionToRocket(state, _currentMissionData);
    HideAcceptMission();
    _avialibleRocketsPanel.SetActive(false);
    missionInfoPanel.SetActive(false);
    _rocketHubPanel.gameObject.SetActive(false);
    _infoPanel.gameObject.SetActive(false);

    StartCoroutine(AnimateMissionLaunch(state));
}

private IEnumerator AnimateMissionLaunch(RocketState state)
{
    // 1. Найдём объект ракеты
    GameObject rocketObj = state.rocketData.rocketPrefab;
    if (rocketObj == null)
    {
        Debug.LogWarning("Rocket prefab not found for animation");
        yield break;
    }

    Debug.Log("Ienumerator started", rocketObj);

    // 2. Отключим управление камерой
    Camera mainCamera = Camera.main;
    TopDownCameraController camController = mainCamera.GetComponent<TopDownCameraController>();
    if (camController != null)
        camController.enabled = false;

    // 3. Переместим камеру к ракете так, чтобы ракета была немного выше центра экрана
    Vector3 rocketPos = rocketObj.transform.position;

    // Смещение камеры назад по направлению взгляда (в плоскости XZ)
    Vector3 offsetDir = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z).normalized;
    float offsetDistance = 30f; // Расстояние смещения — регулируется по вкусу

    Vector3 targetXZ = new Vector3(
        rocketPos.x,
        mainCamera.transform.position.y,
        rocketPos.z
    ) - offsetDir * offsetDistance;

    float moveSpeed = 100f;
    while (Vector3.Distance(new Vector3(mainCamera.transform.position.x, 0, mainCamera.transform.position.z),
                            new Vector3(targetXZ.x, 0, targetXZ.z)) > 0.1f)
    {
        Vector3 newPos = Vector3.MoveTowards(mainCamera.transform.position, targetXZ, moveSpeed * Time.deltaTime);
        mainCamera.transform.position = new Vector3(newPos.x, mainCamera.transform.position.y, newPos.z);
        yield return null;
    }

    // 4. Включим все дочерние Particle объекты
    foreach (Transform child in rocketObj.transform)
    {
        Debug.Log($"TryingFind Child in {rocketObj.name}, name: {child.name}", child);

        if (child.CompareTag("Particle"))
        {
            Debug.Log($"Found: {child.gameObject.name}");
            child.gameObject.SetActive(true);
            Debug.Log($"Set true: {child.gameObject.name}");
        }
    }

    Debug.Log("RocketMoving");

    // 5. Двигать ракету вверх по Y до 100
    float moveUpSpeed = 10f;
    while (rocketObj.transform.position.y < 100f)
    {
        rocketObj.transform.position += Vector3.up * moveUpSpeed * Time.deltaTime;
        yield return null;
    }

    // 6. Включить обратно управление камерой
    if (camController != null)
        camController.enabled = true;
}

    public void BuyRocket(GameObject button)
    {
        RocketButton rocketButton = button.GetComponentInChildren<RocketButton>();
        RocketData rocketData = rocketButton.rocketData;
        foreach (RocketState rocket in RocketHubController.Instance.allRockets)
        {
            if (rocket.rocketData == rocketData)
            {
                RocketHubController.Instance.BuyRocket(rocket);
            }
        }
    }

    public void ShowInfoPanel()
    {
        _infoPanel.gameObject.SetActive(true);
    }
    
    private void SetupInfoPanel(BuildingData data)
    {
        _panels.Clear();
        _infoName.text = data.name;
        _infoText.text = data.buildingInfo;
        _panels.Add(_infoPanel);
        if (data.buildingType == BuildingType.Static)
        {
            _currentPanel = 0;
            _nextButton.gameObject.SetActive(false);
        }
        else if (data.buildingType == BuildingType.RocketHub)
        {
            _currentPanel = 0;
            _panels.Add(_rocketHubPanel);
            _nextButton.gameObject.SetActive(true);
        }

        Debug.Log($"Panels count: {_panels.Count}");
        
        foreach (RectTransform panel in _panels)
        {
            Debug.Log($"Panel: {panel}", panel.gameObject);
        }
    }
}

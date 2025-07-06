using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PowerLines.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [Header("Upgrade Panel")]
    [SerializeField] private RectTransform contentUpgrades;
    [SerializeField] private GameObject upgradeCardPrefab; // Префаб карточки апгрейда с TMP и кнопкой

    private bool _upgradesInitialized = false;

    private readonly string[] upgradeNames = new string[]
    {
        "Advanced Fuel Compression",
        "Automated Launch Sequence",
        "Enhanced Radar Systems",
        "Reinforced Hull Plating",
        "Turbocharger Calibration",
        "Cargo Bay Expansion",
        "Emergency Repair Drones",
        "Thermal Shield Upgrade",
        "Quantum Navigation Core",
        "Faster Mission Dispatch",
        "Defensive Turret AI",
        "Gravity Stabilizer Field",
        "Mission Reward Boost",
        "Engine Preheat Module",
        "Universal Docking Adapter"
    };
    
    [SerializeField] private TextMeshProUGUI _moneyText;

    [SerializeField] private RectTransform _popupNoMoney;
    
    [SerializeField] private AudioClip _rocketSound;
    [SerializeField] private AudioClip _rocketBackSound;
    [SerializeField] private AudioClip _winSound;
    
    [SerializeField] private GameObject _avialibleRocketsPanel;

    [Header("Win Panel")] 
    [SerializeField] private RectTransform _winPanel;

    [SerializeField] private TextMeshProUGUI _missionNameText;
    [SerializeField] private TextMeshProUGUI _planetText;
    [SerializeField] private TextMeshProUGUI _winCountText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    
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

    [SerializeField] private RectTransform _garagePanel;
    [SerializeField] private RectTransform _laboratoryPanel;

    private Vector3 _infoButtonOriginalPos;
    private Vector3 _infoButtonHiddenPos;
    private Tween _infoButtonTween;
    
    private Coroutine _activeMissionTimerCoroutine;
    
    private Dictionary<MissionData, Coroutine> _activeMissionTimers = new Dictionary<MissionData, Coroutine>();
    
    private Dictionary<GameObject, float> _rocketOriginalY = new Dictionary<GameObject, float>();
    
    private BuildingData _currentBuildingData;
    private MissionData _currentMissionData;

    private List<RectTransform> _panels;
    private int _currentPanel;
    
    private void Awake()
    {
        Instance = this;

        // Сохраняем финальную позицию
        _infoButtonOriginalPos = _infoButton.anchoredPosition;

        // Скрытая позиция — ниже экрана (например, на 200 пикселей вниз)
        _infoButtonHiddenPos = _infoButtonOriginalPos - new Vector3(0, 200f, 0);

        // Сразу перемещаем кнопку в скрытую позицию
        _infoButton.anchoredPosition = _infoButtonHiddenPos;

        // Делаем невидимой
        _infoButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        _panels = new List<RectTransform>();
        SetupUpgradePanel();
    }

    private void Update()
    {
        _moneyText.text = $"{RocketHubController.Instance.money}$";
    }
    
    public void SetupUpgradePanel()
    {
        if (_upgradesInitialized) return; // Чтобы не создавать дубли

        foreach (string upgradeName in upgradeNames)
        {
            GameObject card = Instantiate(upgradeCardPrefab, contentUpgrades);

            // Установим текст на карточке
            TextMeshProUGUI tmp = card.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = upgradeName;
            }
            else
            {
                Debug.LogWarning("TMP Text not found in upgrade card prefab");
            }

            // Найдём кнопку в дочерних объектах
            Button btn = card.GetComponentInChildren<Button>();
            if (btn == null)
            {
                Debug.LogWarning("Button not found in upgrade card prefab");
                continue;
            }

            // Проверим, был ли куплен апгрейд ранее
            bool isBought = PlayerPrefs.GetInt("Upgrade_" + upgradeName, 0) == 1;

            if (isBought)
            {
                // Скрываем визуальный элемент апгрейда (например, иконку или кнопку)
                Transform upgradeObj = null;
                foreach (Transform child in card.transform)
                {
                    if (child.CompareTag("Upgrade"))
                    {
                        upgradeObj = child;
                        break;
                    }
                }

                if (upgradeObj != null)
                    upgradeObj.gameObject.SetActive(false);
            }
            else
            {
                // Создаём локальные копии переменных для лямбды
                string thisUpgradeName = upgradeName;
                GameObject thisCard = card;

                // Назначаем обработчик покупки
                btn.onClick.AddListener(() => BuyUpgrade(thisCard, thisUpgradeName));
            }
        }

        _upgradesInitialized = true;
    }

    public void BuyUpgrade(GameObject upgradeCard, string upgradeName)
    {
        int upgradeCost = 100;

        if (RocketHubController.Instance.money >= upgradeCost)
        {
            RocketHubController.Instance.money -= upgradeCost;
            PlayerPrefs.SetInt("money", RocketHubController.Instance.money);
            PlayerPrefs.SetInt("Upgrade_" + upgradeName, 1); // 💾 Сохраняем покупку
            PlayerPrefs.Save();

            // Скрываем визуальный элемент апгрейда
            Transform upgradeObj = null;
            foreach (Transform child in upgradeCard.transform)
            {
                if (child.CompareTag("Upgrade"))
                {
                    upgradeObj = child;
                    break;
                }
            }

            if (upgradeObj != null)
                upgradeObj.gameObject.SetActive(false);
            else
                Debug.LogWarning("No child with tag 'Upgrade' found in upgrade card");

            Debug.Log($"Upgrade bought: {upgradeName}");
        }
        else
        {
            _popupNoMoney.gameObject.SetActive(true);
            Debug.Log("Not enough money to buy upgrade");
        }
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
    
    public void AnimateShowInfoButton()
    {
        if (_infoButtonTween != null && _infoButtonTween.IsActive()) _infoButtonTween.Kill();

        _infoButton.gameObject.SetActive(true);
        _infoButton.anchoredPosition = _infoButtonHiddenPos;

        _infoButtonTween = _infoButton.DOAnchorPos(_infoButtonOriginalPos, 0.5f).SetEase(Ease.OutBack);
    }

    public void AnimateHideInfoButton()
    {
        if (_infoButtonTween != null && _infoButtonTween.IsActive()) _infoButtonTween.Kill();

        _infoButtonTween = _infoButton.DOAnchorPos(_infoButtonHiddenPos, 0.4f)
            .SetEase(Ease.InBack)
            .OnComplete(() => _infoButton.gameObject.SetActive(false));
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

    GameObject rocketObj = state.rocketData.neededRocket;
    if (rocketObj != null)
    {
        if (!_rocketOriginalY.ContainsKey(rocketObj))
            _rocketOriginalY.Add(rocketObj, rocketObj.transform.position.y);
    }

    // Включаем таймер UI
    if (_currentMissionData.timerBackground != null)
        _currentMissionData.timerBackground.gameObject.SetActive(true);

    // Если для этой миссии уже есть запущенный таймер — останавливаем его
    if (_activeMissionTimers.ContainsKey(_currentMissionData))
    {
        StopCoroutine(_activeMissionTimers[_currentMissionData]);
        _activeMissionTimers.Remove(_currentMissionData);
    }

    // Запускаем таймер для этой миссии и запоминаем корутину
    Coroutine timerCoroutine = StartCoroutine(StartMissionTimer(_currentMissionData, rocketObj));
    _activeMissionTimers.Add(_currentMissionData, timerCoroutine);

    // Запускаем анимацию запуска
    StartCoroutine(AnimateMissionLaunch(state));
}

private IEnumerator StartMissionTimer(MissionData mission, GameObject rocketObj)
{
    float timeRemaining = 180f; // 50 секунд или 3 минуты — поправь по необходимости

    while (timeRemaining > 0f)
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        if (mission.timerText != null)
            mission.timerText.text = $"{minutes}:{seconds:00}";

        timeRemaining -= Time.deltaTime;
        yield return null;
    }

    // Таймер закончился — выключаем фон и обнуляем текст
    if (mission.timerBackground != null)
        mission.timerBackground.gameObject.SetActive(false);

    if (mission.timerText != null)
        mission.timerText.text = "0:00";

    // Удаляем корутину из словаря, чтобы не засорять
    _activeMissionTimers.Remove(mission);

    // Возвращаем ракету в исходную позицию по Y
    if (rocketObj != null && _rocketOriginalY.ContainsKey(rocketObj))
    {
        float originalY = _rocketOriginalY[rocketObj];
        StartCoroutine(ReturnRocketToOriginalHeight(rocketObj, originalY, mission));
    }
}
    
private IEnumerator ReturnRocketToOriginalHeight(GameObject rocketObj, float originalY, MissionData missionData)
{
    MusicController.Instance.PlaySpecificSound(_rocketBackSound);
    float speed = 10f;
    Vector3 pos = rocketObj.transform.position;

    while (pos.y > originalY)
    {
        float deltaY = speed * Time.deltaTime;
        pos.y = Mathf.Max(pos.y - deltaY, originalY);
        rocketObj.transform.position = pos;
        yield return null;
    }

    foreach (Transform child in rocketObj.transform)
    {
        if (child.CompareTag("Particle"))
        {
            child.gameObject.SetActive(false);
        }
    }
    
    SetWinPanel(missionData);
}
    
private IEnumerator AnimateMissionLaunch(RocketState state)
{
    // 1. Найдём объект ракеты
    GameObject rocketObj = state.rocketData.neededRocket;
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
    MusicController.Instance.PlaySpecificSound(_rocketSound);
    float totalTime = 10f;
    float endY = 100f;
    float moveUpSpeed = (endY - rocketObj.transform.position.y) / totalTime;
    float timer = 0f;

    while (rocketObj.transform.position.y < endY)
    {
        float deltaY = moveUpSpeed * Time.deltaTime;
        rocketObj.transform.position += Vector3.up * deltaY;
        timer += Time.deltaTime;
        yield return null;
    }
    

    // 6. Включить обратно управление камерой
    if (camController != null)
        camController.enabled = true;
}

    public void BuyRocket(GameObject button)
    {
        if (RocketHubController.Instance.money >= 500)
        {
            RocketHubController.Instance.money -= 500;
            PlayerPrefs.SetInt("money", RocketHubController.Instance.money);
            PlayerPrefs.Save();
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
        else
        {
            _popupNoMoney.gameObject.SetActive(true);
        }
    }

    public void SetWinPanel(MissionData missionData)
    {
        int reward = Random.Range(100, 500);
        RocketHubController.Instance.money += reward;
        
        PlayerPrefs.SetInt("money", RocketHubController.Instance.money);
        PlayerPrefs.Save();
        
        _missionNameText.text = missionData.missionName;
        _planetText.text = missionData.place;
        _winCountText.text = $"You got {reward}$";

        int randomDescription = Random.Range(0, 100);
        
        if (randomDescription <= 50)
        {
            _descriptionText.text = missionData.goodDeliver;
        }
        else
        {
            _descriptionText.text = missionData.badDeliver;
        }
        
        Debug.Log($"Good: {missionData.goodDeliver} Bad: {missionData.badDeliver}");
        
        _winPanel.gameObject.SetActive(true);
        if (_winSound != null)
            MusicController.Instance.PlaySpecificSound(_winSound);
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
            _rocketHubPanel.gameObject.SetActive(false);
            _garagePanel.gameObject.SetActive(false);
        }
        else if (data.buildingType == BuildingType.RocketHub)
        {
            _currentPanel = 0;
            _panels.Add(_rocketHubPanel);
            _nextButton.gameObject.SetActive(true);
            _garagePanel.gameObject.SetActive(false);
            _laboratoryPanel.gameObject.SetActive(false);
        }
        else if (data.buildingType == BuildingType.Garage)
        {
            _currentPanel = 0;
            _panels.Add(_garagePanel);
            _nextButton.gameObject.SetActive(true);
            _rocketHubPanel.gameObject.SetActive(false);
            _laboratoryPanel.gameObject.SetActive(false);
        }
        else if (data.buildingType == BuildingType.Laboratory)
        {
            _currentPanel = 0;
            _panels.Add(_laboratoryPanel);
            _nextButton.gameObject.SetActive(true);
            _rocketHubPanel.gameObject.SetActive(false);
            _garagePanel.gameObject.SetActive(false);
        }

        Debug.Log($"Panels count: {_panels.Count}");
        
        foreach (RectTransform panel in _panels)
        {
            Debug.Log($"Panel: {panel}", panel.gameObject);
        }
    }
}

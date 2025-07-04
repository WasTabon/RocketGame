using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class RocketState
{
    public bool isPurchased;
    public bool isArrived;
    public RocketData rocketData;
    public MissionData assignedMission;
}

public class RocketHubController : MonoBehaviour
{
    public static RocketHubController Instance;

    public event Action<RocketState, MissionData> OnMissionAssigned;
    public event Action<RocketState, MissionData> OnMissionCompleted;
    public event Action<RocketState> OnRocketBuy;

    [SerializeField] private RocketState _astrovan = new RocketState();
    [SerializeField] private RocketState _cargo = new RocketState();
    [SerializeField] private RocketState _iron = new RocketState();
    [SerializeField] private RocketState _magnetox = new RocketState();
    [SerializeField] private RocketState _nimbus = new RocketState();
    [SerializeField] private RocketState _plasma = new RocketState();
    [SerializeField] private RocketState _quantum = new RocketState();
    [SerializeField] private RocketState _stealth = new RocketState();
    [SerializeField] private RocketState _vortex = new RocketState();
    [SerializeField] private RocketState _zoomster = new RocketState();

    public int money;
    
    public List<RocketState> allRockets;
    
    private Dictionary<RocketData, GameObject> _spawnedRockets = new();

    #region Rockets

    public RocketState Astrovan => _astrovan;
    public RocketState Cargo => _cargo;
    public RocketState Iron => _iron;
    public RocketState Magnetox => _magnetox;
    public RocketState Nimbus => _nimbus;
    public RocketState Plasma => _plasma;
    public RocketState Quantum => _quantum;
    public RocketState Stealth => _stealth;
    public RocketState Vortex => _vortex;
    public RocketState Zoomster => _zoomster;

    public void SetAstrovan(bool purchased, bool arrived) { _astrovan.isPurchased = purchased; _astrovan.isArrived = arrived; }
    public void SetCargo(bool purchased, bool arrived) { _cargo.isPurchased = purchased; _cargo.isArrived = arrived; }
    public void SetIron(bool purchased, bool arrived) { _iron.isPurchased = purchased; _iron.isArrived = arrived; }
    public void SetMagnetox(bool purchased, bool arrived) { _magnetox.isPurchased = purchased; _magnetox.isArrived = arrived; }
    public void SetNimbus(bool purchased, bool arrived) { _nimbus.isPurchased = purchased; _nimbus.isArrived = arrived; }
    public void SetPlasma(bool purchased, bool arrived) { _plasma.isPurchased = purchased; _plasma.isArrived = arrived; }
    public void SetQuantum(bool purchased, bool arrived) { _quantum.isPurchased = purchased; _quantum.isArrived = arrived; }
    public void SetStealth(bool purchased, bool arrived) { _stealth.isPurchased = purchased; _stealth.isArrived = arrived; }
    public void SetVortex(bool purchased, bool arrived) { _vortex.isPurchased = purchased; _vortex.isArrived = arrived; }
    public void SetZoomster(bool purchased, bool arrived) { _zoomster.isPurchased = purchased; _zoomster.isArrived = arrived; }

    #endregion

    #region Missions
    private bool _noodles;
    private bool _blackMatter;
    private bool _cryoPet;
    private bool _interdimensional;
    private bool _package;
    private bool _boxShape;
    private bool _infrared;
    private bool _laser;
    private bool _secret;
    private bool _pacifier;

    public bool Noodles => _noodles;
    public bool BlackMatter => _blackMatter;
    public bool CryoPet => _cryoPet;
    public bool Interdimensional => _interdimensional;
    public bool Package => _package;
    public bool BoxShape => _boxShape;
    public bool Infrared => _infrared;
    public bool Laser => _laser;
    public bool Secret => _secret;
    public bool Pacifier => _pacifier;
    #endregion

    public int _initRockets;

    private void Awake()
    {
        Instance = this;

        allRockets = new List<RocketState>
        {
            _astrovan, _cargo, _iron, _magnetox, _nimbus,
            _plasma, _quantum, _stealth, _vortex, _zoomster
        };
        
        //LoadRocketStates();
        //UpdateRocketVisualLocks();

        _zoomster.isPurchased = true;
        _zoomster.isArrived = false;
    }

    private void Update()
    {
        if (_initRockets == 10)
        {
            _initRockets++;
            LoadRocketStates();
            UpdateRocketVisualLocks();
        }
    }

    public void BuyRocket(RocketState rocket)
    {
        Debug.Log("Rocket buyed");
        rocket.isPurchased = true;

        if (rocket.rocketData.locked != null)
            rocket.rocketData.locked.gameObject.SetActive(false);

        SpawnRocketOnPlatform(rocket);

        OnRocketBuy?.Invoke(rocket);
        SaveRocketStates();
    }

    public void UpdateRocketVisualLocks()
    {
        foreach (var rocket in allRockets)
        {
            if (rocket == _zoomster)
                continue;
            
            if (rocket.rocketData.locked != null)
                rocket.rocketData.locked.gameObject.SetActive(!rocket.isPurchased);
        }
    }

    public void SaveRocketStates()
    {
        foreach (var rocket in allRockets)
        {
            PlayerPrefs.SetInt("Rocket_" + rocket.rocketData.rocketName + "_purchased", rocket.isPurchased ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    public void LoadRocketStates()
    {
        foreach (var rocket in allRockets)
        {
            if (rocket == _zoomster)
                continue;

            string key = "Rocket_" + rocket.rocketData.rocketName + "_purchased";

            if (PlayerPrefs.HasKey(key))
            {
                rocket.isPurchased = PlayerPrefs.GetInt(key) == 1;
            }
            else
            {
                rocket.isPurchased = (rocket == _zoomster);
            }
        }

        // После загрузки данных — заспавни все купленные ракеты
        foreach (var rocket in allRockets)
        {
            if (rocket.isPurchased)
            {
                SpawnRocketOnPlatform(rocket);
            }
        }
        
        // додати анімацію для кнопки, додати лабораторію і покупку ракет за валюту і попап при місії і винагороди за місії
    }

    public List<RocketState> GetAvialibleRockets()
    {
        List<RocketState> available = new List<RocketState>();

        foreach (var rocket in allRockets)
        {
            if (rocket.isPurchased && !rocket.isArrived)
                available.Add(rocket);
        }

        return available;
    }

    public void AssignMissionToRocket(RocketState rocket, MissionData mission)
    {
        rocket.assignedMission = mission;
        rocket.isArrived = true;

        OnMissionAssigned?.Invoke(rocket, mission);
        StartCoroutine(CompleteMissionAfterDelay(rocket, mission, 50f));
    }
    
    public GameObject GetSpawnedRocket(RocketData data)
    {
        return _spawnedRockets.ContainsKey(data) ? _spawnedRockets[data] : null;
    }

    private IEnumerator CompleteMissionAfterDelay(RocketState rocket, MissionData mission, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);

        rocket.isArrived = false;
        rocket.assignedMission = null;

        OnMissionCompleted?.Invoke(rocket, mission);
    }
    
    private void SpawnRocketOnPlatform(RocketState rocket)
    {
        if (rocket.rocketData.rocketPrefab == null)
        {
            Debug.LogWarning($"Rocket prefab not set for {rocket.rocketData.rocketName}");
            return;
        }
        if (rocket.rocketData.platform == null)
        {
            Debug.LogWarning($"Platform prefab not set for {rocket.rocketData.rocketName}");
            return;
        }

        // Удаляем старую ракету, если уже есть
        if (_spawnedRockets.ContainsKey(rocket.rocketData))
            Destroy(_spawnedRockets[rocket.rocketData]);

        // Инстанцируем объект
        GameObject rocketInstance = Instantiate(rocket.rocketData.rocketPrefab);
        rocketInstance.transform.rotation = rocket.rocketData.platform.transform.rotation;

        //rocket.rocketData.rocketPrefab = rocketInstance;
        rocket.rocketData.neededRocket = rocketInstance;

        // Получаем рендереры
        Renderer rocketRenderer = rocketInstance.GetComponentInChildren<Renderer>();
        Renderer platformRenderer = rocket.rocketData.platform.GetComponentInChildren<Renderer>();

        if (rocketRenderer == null || platformRenderer == null)
        {
            Debug.LogWarning("Missing renderer on rocket or platform");
            return;
        }

        // Границы
        Bounds rocketBounds = rocketRenderer.bounds;
        Bounds platformBounds = platformRenderer.bounds;

        // Центр платформы по X и Z, верх платформы по Y
        Vector3 platformTopCenter = new Vector3(
            platformBounds.center.x,
            platformBounds.max.y,
            platformBounds.center.z
        );

        // Центр нижней части ракеты (по Y — низ, по XZ — центр)
        Vector3 rocketBottomCenter = new Vector3(
            rocketBounds.center.x,
            rocketBounds.min.y,
            rocketBounds.center.z
        );

        // Смещение, чтобы нижняя часть ракеты стала в верх платформы, по центру
        Vector3 offset = platformTopCenter - rocketBottomCenter;

        // Устанавливаем позицию
        rocketInstance.transform.position += offset;

        _spawnedRockets[rocket.rocketData] = rocketInstance;
    }
    
    void OnApplicationQuit()
    {
        SaveEverything();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveEverything();
    }

    void OnApplicationFocus(bool focus)
    {
        if (!focus)
            SaveEverything();
    }

    void SaveEverything()
    {
        
    }
}

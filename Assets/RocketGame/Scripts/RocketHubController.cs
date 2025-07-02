using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private List<RocketState> _allRockets;

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

    #region
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

    private void Awake()
    {
        Instance = this;

        _allRockets = new List<RocketState>
        {
            _astrovan,
            _cargo,
            _iron,
            _magnetox,
            _nimbus,
            _plasma,
            _quantum,
            _stealth,
            _vortex,
            _zoomster
        };
    }

    public List<RocketState> GetAvialibleRockets()
    {
        List<RocketState> available = new List<RocketState>();

        foreach (var rocket in _allRockets)
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
        StartCoroutine(CompleteMissionAfterDelay(rocket, mission, 180f));
    }

    private IEnumerator CompleteMissionAfterDelay(RocketState rocket, MissionData mission, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);

        rocket.isArrived = false;
        rocket.assignedMission = null;

        OnMissionCompleted?.Invoke(rocket, mission);
    }
}

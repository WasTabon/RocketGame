using System;
using UnityEngine;

[System.Serializable]
public class RocketState
{
    public bool isPurchased;
    public bool isArrived;
    public RocketData rocketData;
}

public class RocketHubController : MonoBehaviour
{
    public static RocketHubController Instance;
    
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

    #region Rockets
    
    public RocketState Zoomster => _zoomster;
    public RocketState Cargo => _cargo;
    public RocketState Plasma => _plasma;
    public RocketState Iron => _iron;
    public RocketState Quantum => _quantum;
    public RocketState Nimbus => _nimbus;
    public RocketState Vortex => _vortex;
    public RocketState Magnetox => _magnetox;
    public RocketState Astrovan => _astrovan;
    public RocketState Stealth => _stealth;

    public void SetZoomster(bool purchased, bool arrived) { _zoomster.isPurchased = purchased; _zoomster.isArrived = arrived; }
    public void SetCargo(bool purchased, bool arrived) { _cargo.isPurchased = purchased; _cargo.isArrived = arrived; }
    public void SetPlasma(bool purchased, bool arrived) { _plasma.isPurchased = purchased; _plasma.isArrived = arrived; }
    public void SetIron(bool purchased, bool arrived) { _iron.isPurchased = purchased; _iron.isArrived = arrived; }
    public void SetQuantum(bool purchased, bool arrived) { _quantum.isPurchased = purchased; _quantum.isArrived = arrived; }
    public void SetNimbus(bool purchased, bool arrived) { _nimbus.isPurchased = purchased; _nimbus.isArrived = arrived; }
    public void SetVortex(bool purchased, bool arrived) { _vortex.isPurchased = purchased; _vortex.isArrived = arrived; }
    public void SetMagnetox(bool purchased, bool arrived) { _magnetox.isPurchased = purchased; _magnetox.isArrived = arrived; }
    public void SetAstrovan(bool purchased, bool arrived) { _astrovan.isPurchased = purchased; _astrovan.isArrived = arrived; }
    public void SetStealth(bool purchased, bool arrived) { _stealth.isPurchased = purchased; _stealth.isArrived = arrived; }

    #endregion

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

    #region Missions

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

    public void SetNoodles(bool value) => _noodles = value;
    public void SetBlackMatter(bool value) => _blackMatter = value;
    public void SetCryoPet(bool value) => _cryoPet = value;
    public void SetInterdimensional(bool value) => _interdimensional = value;
    public void SetPackage(bool value) => _package = value;
    public void SetBoxShape(bool value) => _boxShape = value;
    public void SetInfrared(bool value) => _infrared = value;
    public void SetLaser(bool value) => _laser = value;
    public void SetSecret(bool value) => _secret = value;
    public void SetPacifier(bool value) => _pacifier = value;

    #endregion

    private void Awake()
    {
        Instance = this;
    }

    public void ShowAvialibleRockets()
    {
        
    }
}

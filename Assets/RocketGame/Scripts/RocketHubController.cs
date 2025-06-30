using UnityEngine;

public class RocketHubController : MonoBehaviour
{
    private bool _zoomster;
    private bool _cargo;
    private bool _plasma;
    private bool _iron;
    private bool _quantum;
    private bool _nimbus;
    private bool _vortex;
    private bool _magnetox;
    private bool _astrovan;
    private bool _stealth;

    #region Rockets

    public bool Zoomster => _zoomster;
    public bool Cargo => _cargo;
    public bool Plasma => _plasma;
    public bool Iron => _iron;
    public bool Quantum => _quantum;
    public bool Nimbus => _nimbus;
    public bool Vortex => _vortex;
    public bool Magnetox => _magnetox;
    public bool Astrovan => _astrovan;
    public bool Stealth => _stealth;

    public void SetZoomster(bool value) => _zoomster = value;
    public void SetCargo(bool value) => _cargo = value;
    public void SetPlasma(bool value) => _plasma = value;
    public void SetIron(bool value) => _iron = value;
    public void SetQuantum(bool value) => _quantum = value;
    public void SetNimbus(bool value) => _nimbus = value;
    public void SetVortex(bool value) => _vortex = value;
    public void SetMagnetox(bool value) => _magnetox = value;
    public void SetAstrovan(bool value) => _astrovan = value;
    public void SetStealth(bool value) => _stealth = value;

    #endregion
}
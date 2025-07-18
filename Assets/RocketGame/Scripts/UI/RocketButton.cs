using PowerLines.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class RocketButton : MonoBehaviour
{
    public RocketData rocketData;
    public RectTransform _locked;
    [SerializeField] private GameObject _platform;
    [SerializeField] private GameObject _rocket;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowRocketInfo);

        rocketData.locked = _locked;
        rocketData.platform = _platform;
        if (_rocket != null)
        {
            rocketData.rocketPrefab = _rocket;
        }

        RocketHubController.Instance._initRockets++;
        Debug.Log($"Rockets init: {RocketHubController.Instance._initRockets}");
    }

    private void ShowRocketInfo()
    {
        MusicController.Instance.PlayClickSound();
        UIController.Instance.ShowRocketInfo(rocketData);
    }
}
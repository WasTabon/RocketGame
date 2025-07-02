using UnityEngine;
using UnityEngine.UI;

public class RocketButton : MonoBehaviour
{
    public RocketData rocketData;
    public RectTransform _locked;
    [SerializeField] private GameObject _platform;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowRocketInfo);

        rocketData.locked = _locked;
        rocketData.platform = _platform;
    }

    private void ShowRocketInfo()
    {
        UIController.Instance.ShowRocketInfo(rocketData);
    }
}
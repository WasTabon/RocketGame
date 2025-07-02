using UnityEngine;
using UnityEngine.UI;

public class RocketButton : MonoBehaviour
{
    [SerializeField] private RocketData rocketData;
    [SerializeField] private RectTransform _locked;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);

        rocketData.locked = _locked;
    }

    private void OnClick()
    {
        UIController.Instance.ShowRocketInfo(rocketData);
    }
}
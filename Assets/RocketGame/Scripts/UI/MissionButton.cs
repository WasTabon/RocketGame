using UnityEngine;
using UnityEngine.UI;

public class MissionButton : MonoBehaviour
{
    [SerializeField] private RocketData rocketData;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        UIController.Instance.ShowRocketInfo(rocketData);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class RocketButton : MonoBehaviour
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
using System;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private RectTransform _infoButton;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowInfoButton(BuildingData data)
    {
        _infoButton.gameObject.SetActive(true);
    }
}

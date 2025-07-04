using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _panelsToDeactivate;

    private void Start()
    {
        foreach (GameObject panel in _panelsToDeactivate)
        {
            panel.SetActive(true);
        }
        
        StartCoroutine(DeactivatePanelsAfterFrames(5));
    }

    private IEnumerator DeactivatePanelsAfterFrames(int frameCount)
    {
        for (int i = 0; i < frameCount; i++)
        {
            yield return null;
        }

        foreach (GameObject panel in _panelsToDeactivate)
        {
            panel.SetActive(false);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class MissionButton : MonoBehaviour
{
    [SerializeField] private MissionData _missionData;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (_missionData != null)
            UIController.Instance.ShowMissionInfo(_missionData);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_missionData != null && transform.parent != null)
        {
            transform.parent.name = _missionData.name;
        }
    }
#endif

    /// <summary>
    /// Устанавливает _missionData.name в первый TextMeshProUGUI среди дочерних объектов родителя.
    /// </summary>
    [ContextMenu("Assign Name To First TMP")]
    public void AssignNameToFirstTMP()
    {
        if (_missionData == null)
        {
            Debug.LogWarning("MissionData is not assigned.");
            return;
        }

        var parent = transform.parent;
        if (parent == null)
        {
            Debug.LogWarning("No parent object found.");
            return;
        }

        var tmp = parent.GetComponentInChildren<TextMeshProUGUI>(true);
        if (tmp != null)
        {
            tmp.text = _missionData.name;
#if UNITY_EDITOR
            EditorUtility.SetDirty(tmp); // Помечает объект как изменённый (для сохранения в сцене)
#endif
        }
        else
        {
            Debug.LogWarning("No TextMeshProUGUI found in parent children.");
        }
    }
}
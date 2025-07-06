using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickDetector : MonoBehaviour
{
    public static ClickDetector Instance;

    [Header("Настройки")]
    public LayerMask clickLayerMask = ~0;
    public UnityEvent<Building> OnBuildingClicked;
    public UnityEvent<Vector3> OnEmptyClicked;

    private Vector2 startPos;
    private float dragThreshold = 10f;
    private bool touchSupported;

    void Awake()
    {
        Instance = this;

#if UNITY_EDITOR
        touchSupported = false;
#else
        touchSupported = true;
#endif
    }

    void Update()
    {
        if (touchSupported)
            HandleTouch();
        else
            HandleMouse();
    }

    void HandleTouch()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            startPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            if (Vector2.Distance(startPos, touch.position) < dragThreshold)
            {
                if (!IsPointerOverUI(touch.fingerId))
                    DetectClick(touch.position);
            }
        }
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Vector2.Distance(startPos, Input.mousePosition) < dragThreshold)
            {
                if (!IsPointerOverUI())
                    DetectClick(Input.mousePosition);
            }
        }
    }

    bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    bool IsPointerOverUI(int fingerId)
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(fingerId);
    }

    void DetectClick(Vector2 screenPos)
    {
        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, clickLayerMask))
        {
            var building = hit.collider.GetComponent<Building>();
            if (building != null)
            {
                OnBuildingClicked?.Invoke(building);
                building.Click();
                UIController.Instance.AnimateShowInfoButton();
            }
            else
            {
                OnEmptyClicked?.Invoke(hit.point);
                UIController.Instance.AnimateHideInfoButton();
            }
        }
        else
        {
            Vector3 fallbackPoint = ray.origin + ray.direction * 10f;
            OnEmptyClicked?.Invoke(fallbackPoint);
            UIController.Instance.AnimateHideInfoButton();
        }
    }
}

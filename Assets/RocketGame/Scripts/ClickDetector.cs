using UnityEngine;
using UnityEngine.Events;

public class ClickDetector : MonoBehaviour
{
    [Header("Настройки")]
    public LayerMask clickLayerMask = ~0; // Какие слои обрабатывать (по умолчанию все)
    public UnityEvent<Building> OnBuildingClicked;
    public UnityEvent<Vector3> OnEmptyClicked;

    private Vector2 startPos;
    private float dragThreshold = 10f; // в пикселях
    private bool touchSupported;

    void Awake()
    {
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
                DetectClick(touch.position);
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
                DetectClick(Input.mousePosition);
        }
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
                Debug.Log($"Click on {building.gameObject.name}", building.gameObject);
            }
            else
            {
                OnEmptyClicked?.Invoke(hit.point);
                Debug.Log($"Empty click on {hit.point}");
            }
        }
        else
        {
            Vector3 fallbackPoint = ray.origin + ray.direction * 10f;
            OnEmptyClicked?.Invoke(fallbackPoint);
            Debug.Log($"Empty click in space at {fallbackPoint}");
        }
    }
}

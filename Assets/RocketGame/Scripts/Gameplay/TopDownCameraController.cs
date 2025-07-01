using UnityEngine;

public class TopDownCameraController : MonoBehaviour
{
    [Header("Настройки")]
    public float panSpeed = 0.2f;           // чувствительность панорамирования
    public float inertiaDamping = 5f;       // затухание инерции
    public float inertiaMultiplier = 0.1f;  // множитель силы инерции

    private Vector3 panVelocity;            // скорость камеры (инерция)
    private bool isPanning;

    private Vector2 lastPanScreenPos;

#if !UNITY_EDITOR
    private bool touchSupported = true;
#else
    private bool touchSupported = false;
#endif

    void Update()
    {
        if (touchSupported)
            HandleTouch();
        else
            HandleMouse();

        if (!isPanning)
        {
            if (panVelocity.magnitude > 0.01f)
            {
                MoveCamera(panVelocity * Time.deltaTime);
                panVelocity = Vector3.Lerp(panVelocity, Vector3.zero, inertiaDamping * Time.deltaTime);
            }
            else
            {
                panVelocity = Vector3.zero;
            }
        }
    }

    void HandleTouch()
    {
        if (Input.touchCount == 0)
        {
            isPanning = false;
            return;
        }

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            isPanning = true;
            lastPanScreenPos = touch.position;
            panVelocity = Vector3.zero;
        }
        else if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && isPanning)
        {
            Vector2 currentPos = touch.position;
            Vector2 deltaScreen = currentPos - lastPanScreenPos;

            Vector3 worldDelta = ScreenDeltaToWorldDelta(deltaScreen);

            MoveCamera(-worldDelta * panSpeed);

            // Рассчитываем скорость инерции по дельте и времени между кадрами
            panVelocity = (-worldDelta / Time.deltaTime) * inertiaMultiplier;

            lastPanScreenPos = currentPos;
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            isPanning = false;
        }
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isPanning = true;
            lastPanScreenPos = Input.mousePosition;
            panVelocity = Vector3.zero;
        }
        else if (Input.GetMouseButton(0) && isPanning)
        {
            Vector2 currentPos = (Vector2)Input.mousePosition;
            Vector2 deltaScreen = currentPos - lastPanScreenPos;

            Vector3 worldDelta = ScreenDeltaToWorldDelta(deltaScreen);

            MoveCamera(-worldDelta * panSpeed);

            panVelocity = (-worldDelta / Time.deltaTime) * inertiaMultiplier;

            lastPanScreenPos = currentPos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPanning = false;
        }
    }

    // Перевод смещения в пикселях на экране в смещение по миру по осям XZ
    Vector3 ScreenDeltaToWorldDelta(Vector2 delta)
    {
        // Получаем направление в мире для осей X и Z относительно камеры
        // Берём вектор вправо и вперёд камеры, проецируем их на плоскость XZ (y=0)
        Vector3 right = Camera.main.transform.right;
        right.y = 0;
        right.Normalize();

        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        forward.Normalize();

        // delta.x влияет на смещение по правой оси, delta.y — по вперёд/назад
        Vector3 worldDelta = right * delta.x + forward * delta.y;

        // Можно масштабировать worldDelta здесь, но у нас есть panSpeed отдельный

        return worldDelta;
    }

    void MoveCamera(Vector3 delta)
    {
        Vector3 newPos = transform.position + delta;
        transform.position = newPos;
    }
}

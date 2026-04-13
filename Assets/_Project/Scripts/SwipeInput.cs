using UnityEngine;

public class SwipeInput : MonoBehaviour
{
    public BolasThrower thrower;
    public float minimumSwipeDistance = 40f;
    public float maximumSwipeDistance = 600f;

    private bool touchTracking;
    private bool mouseTracking;
    private int trackedFingerId = -1;
    private Vector2 swipeStartPosition;

    private void Awake()
    {
        EnsureThrower();
    }

    private void Update()
    {
        EnsureThrower();
        HandleTouchInput();
        HandleMouseInput();
    }

    private void EnsureThrower()
    {
        if (thrower != null)
        {
            return;
        }

        BolasThrower[] throwers = FindObjectsOfType<BolasThrower>();
        if (throwers.Length > 0)
        {
            thrower = throwers[0];
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount <= 0)
        {
            return;
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                trackedFingerId = touch.fingerId;
                swipeStartPosition = touch.position;
                touchTracking = true;
                return;
            }

            if (!touchTracking || touch.fingerId != trackedFingerId)
            {
                continue;
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                CompleteSwipe(touch.position);
                touchTracking = false;
                trackedFingerId = -1;
                return;
            }
        }
    }

    private void HandleMouseInput()
    {
        if (Input.touchCount > 0)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            swipeStartPosition = Input.mousePosition;
            mouseTracking = true;
        }

        if (mouseTracking && Input.GetMouseButtonUp(0))
        {
            CompleteSwipe(Input.mousePosition);
            mouseTracking = false;
        }
    }

    private void CompleteSwipe(Vector2 endPosition)
    {
        if (thrower == null)
        {
            return;
        }

        Vector2 delta = endPosition - swipeStartPosition;
        float distance = delta.magnitude;
        if (distance < minimumSwipeDistance)
        {
            return;
        }

        float normalizedPower = Mathf.Clamp01(distance / maximumSwipeDistance);
        float horizontal = Mathf.Clamp(delta.x / Screen.width, -1f, 1f);
        float vertical = Mathf.Clamp(delta.y / Screen.height, -0.25f, 1f);

        Vector3 direction = new Vector3(
            horizontal * 0.9f,
            0.4f + Mathf.Max(0f, vertical) * 1.2f,
            1f + Mathf.Max(0f, vertical) * 0.4f
        ).normalized;

        float power = Mathf.Lerp(8f, 20f, normalizedPower);
        thrower.Throw(direction, power);
    }
}

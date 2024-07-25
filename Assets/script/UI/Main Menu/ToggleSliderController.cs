using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleSliderController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform slider;  // Reference to the RectTransform of the slider
    public RectTransform upperLimitObject;  // Reference to an object defining the upper limit
    public float speed = 5f;  // Speed for smooth movement
    public float lowerLimit = -100f;  // Lower limit for the slider's movement
    public float upperOffset = 10f;  // Offset for the upper limit
    public float smoothTime = 0.1f;  // Time for smooth damping

    private bool isMoving = false;  // Flag to check if the slider is moving automatically
    private bool isDragging = false;  // Flag to check if the slider is being dragged
    private bool moveUp = true;  // Direction flag for automatic movement
    private Vector3 targetPosition;  // Target position for smooth damping
    private Vector3 velocity = Vector3.zero;  // Velocity used by SmoothDamp

    void Start()
    {
        // Initialize the target position to the slider's current position
        targetPosition = slider.localPosition;

        // Ensure the initial position is within bounds
        float upperLimit = upperLimitObject.localPosition.y - upperOffset;
        targetPosition.y = Mathf.Clamp(targetPosition.y, lowerLimit, upperLimit);
        slider.localPosition = targetPosition;
    }

    void Update()
    {
        if (isMoving && !isDragging)
        {
            float moveAmount = speed * Time.deltaTime * (moveUp ? 1 : -1);
            targetPosition += new Vector3(0, moveAmount, 0);

            float upperLimit = upperLimitObject.localPosition.y - upperOffset;
            if (targetPosition.y > upperLimit)
            {
                targetPosition.y = upperLimit;
                isMoving = false;
                moveUp = false;
            }
            else if (targetPosition.y < lowerLimit)
            {
                targetPosition.y = lowerLimit;
                isMoving = false;
                moveUp = true;
            }
        }

        if (!isDragging)
        {
            // Smoothly move towards the target position using SmoothDamp when not dragging
            slider.localPosition = Vector3.SmoothDamp(slider.localPosition, targetPosition, ref velocity, smoothTime);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDragging = true;

        // Convert screen position to local position
        Vector3 localPosition = GetLocalPositionFromEvent(eventData);

        // Calculate new target position
        float moveAmount = (localPosition.y - slider.localPosition.y);
        targetPosition = slider.localPosition + new Vector3(0, moveAmount, 0);

        // Clamp target position within limits
        float upperLimit = upperLimitObject.localPosition.y - upperOffset;
        targetPosition.y = Mathf.Clamp(targetPosition.y, lowerLimit, upperLimit);

        // Update slider position immediately
        slider.localPosition = targetPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        isMoving = false;  // Stop automatic movement while dragging
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        // Snap to closest limit when pointer is up
        float upperLimit = upperLimitObject.localPosition.y - upperOffset;
        if (slider.localPosition.y > (upperLimit + lowerLimit) / 2)
        {
            targetPosition.y = upperLimit;
        }
        else
        {
            targetPosition.y = lowerLimit;
        }
    }

    private Vector3 GetLocalPositionFromEvent(PointerEventData eventData)
    {
        // Convert screen position to world position
        Vector3 screenPosition = eventData.position;
        screenPosition.z = Mathf.Abs(Camera.main.transform.position.z - slider.position.z);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        // Convert world position to local position
        return slider.parent.InverseTransformPoint(worldPosition);
    }

    public void ToggleSlider()
    {
        if (!isDragging)
        {
            isMoving = !isMoving;
        }
    }
}

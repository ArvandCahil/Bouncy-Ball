using UnityEngine;
using UnityEngine.UI;

public class ToggleSliderController : MonoBehaviour
{
    public RectTransform slider;  
    public RectTransform upperLimitObject; 
    public float speed = 5f;      
    public float lowerLimit = -100f; 
    public float upperOffset = 10f; 

    private bool isMoving = false;
    private bool moveUp = true;

    void Update()
    {
        if (isMoving)
        {
            float moveAmount = speed * Time.deltaTime * (moveUp ? 1 : -1);
            Vector3 newPosition = slider.localPosition + new Vector3(0, moveAmount, 0);

            float upperLimit = upperLimitObject.localPosition.y - upperOffset;

            if (newPosition.y > upperLimit)
            {
                newPosition.y = upperLimit;
                isMoving = false;
                moveUp = false; 
            }
            else if (newPosition.y < lowerLimit)
            {
                newPosition.y = lowerLimit;
                isMoving = false;
                moveUp = true;
            }

            slider.localPosition = newPosition;
        }
    }

    public void ToggleSlider()
    {
        isMoving = !isMoving;
    }
}

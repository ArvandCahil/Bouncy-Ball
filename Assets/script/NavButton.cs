using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonMovementController : MonoBehaviour
{
    public Button[] buttons; 
    public float moveDistance = 50f; 
    public float moveDuration = 0.2f; 

    private Button activeButton = null; 

    void Start()
    {
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClick(button));
        }
    }

    void OnButtonClick(Button clickedButton)
    {
        if (activeButton == clickedButton)
        {           
            return;
        }

        if (activeButton != null)
        {
            StartCoroutine(MoveButton(activeButton.transform, -moveDistance, moveDuration));
        }

        StartCoroutine(MoveButton(clickedButton.transform, moveDistance, moveDuration));

        activeButton = clickedButton;
    }

    IEnumerator MoveButton(Transform buttonTransform, float distance, float duration)
    {
        Vector3 startPosition = buttonTransform.position;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + distance, startPosition.z);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            buttonTransform.position = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        buttonTransform.position = endPosition;
    }
}

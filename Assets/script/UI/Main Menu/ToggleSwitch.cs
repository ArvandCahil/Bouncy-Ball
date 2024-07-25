using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour
{
    public Button toggleButton;
    public RectTransform switchTransform;
    public Color onColor = Color.green;
    public Color offColor = Color.red;
    private bool isOn = false;
    private Vector2 onPosition;
    private Vector2 offPosition;

    void Start()
    {
        toggleButton.onClick.AddListener(Toggle);
        onPosition = new Vector2(47, switchTransform.anchoredPosition.y); 
        offPosition = new Vector2(-47, switchTransform.anchoredPosition.y); 
        UpdateSwitch();
    }

    void Toggle()
    {
        isOn = !isOn;
        UpdateSwitch();
    }

    void UpdateSwitch()
    {
        switchTransform.anchoredPosition = isOn ? onPosition : offPosition;
        switchTransform.GetComponent<Image>().color = isOn ? onColor : offColor;
    }
}

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 

public class MoveObjectsOnButtonPress : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public Button button;
    public float moveDuration = 2.0f; 
    public Ease easeType = Ease.InOutCubic; 

    private Vector3 targetPosition1;
    private Vector3 targetPosition2;
    private Vector3 secondTargetPosition1;
    private Vector3 secondTargetPosition2;
    private Vector3 moveAfterTeleportPositionObject1;
    private Vector3 moveAfterTeleportPositionObject2;

    private int clickCount = 0;

    void Start()
    {
        
        button.onClick.AddListener(OnButtonPress);

        
        targetPosition1 = new Vector3(0, 1075f, 0);
        targetPosition2 = new Vector3(0, 905f, 0);
        secondTargetPosition1 = new Vector3(640f, 735f, 0);
        secondTargetPosition2 = new Vector3(0, 1075f, 0);
        moveAfterTeleportPositionObject1 = new Vector3(640f, 905f, 0); 
        moveAfterTeleportPositionObject2 = new Vector3(640f, 905f, 0); 
    }

    void OnButtonPress()
    {
        
        if (clickCount == 0)
        {
            MoveObjectToPosition(object1, targetPosition1);
            MoveObjectToPosition(object2, targetPosition2);
        }
        else if (clickCount == 1)
        {
            
            DOTween.Sequence()
                .Append(object1.transform.DOMove(secondTargetPosition1, 0f)) 
                .AppendInterval(0.1f) 
                .Append(object1.transform.DOMove(moveAfterTeleportPositionObject1, moveDuration).SetEase(easeType))
                .Play();

            MoveObjectToPosition(object2, secondTargetPosition2);
        }
        else if (clickCount == 2)
        {
            
            DOTween.Sequence()
                .Append(object2.transform.DOMove(new Vector3(645f, 735f, 0), 0f)) 
                .AppendInterval(0.1f) 
                .Append(object2.transform.DOMove(moveAfterTeleportPositionObject2, moveDuration).SetEase(easeType))
                .Play();

            
            MoveObjectToPosition(object1, targetPosition1);

            
            clickCount = 1;
            return; 
        }
        clickCount++;
    }

    void MoveObjectToPosition(GameObject obj, Vector3 targetPosition)
    {
        if (obj != null)
        {
            obj.transform.DOMove(new Vector3(obj.transform.position.x, targetPosition.y, obj.transform.position.z), moveDuration).SetEase(easeType);
        }
    }
}

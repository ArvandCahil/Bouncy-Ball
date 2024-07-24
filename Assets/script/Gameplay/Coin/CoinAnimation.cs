using DG.Tweening;
using UnityEngine;

public class CoinAnimation : MonoBehaviour
{

    void Start()
    {
        // Rotate continuously
        transform.DORotate(new Vector3(0f, 360f, 0f), 2f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);

        // Move up and down smoothly
        transform.DOMoveY(transform.position.y + 0.5f, 2f) // Increase duration for slower movement
            .SetLoops(-1, LoopType.Yoyo) // Yoyo loop for smooth up and down
            .SetEase(Ease.InOutSine); // Sine easing for smooth continuous motion
    }
}

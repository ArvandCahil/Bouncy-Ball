using UnityEngine;

public class PipeController : MonoBehaviour
{
    [SerializeField] private bool isHorizontal;
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public bool IsHorizontal => isHorizontal;

    public float GetPipeLength()
    {
        // Menghitung panjang pipa berdasarkan ukuran collider dan skala objek
        float length = isHorizontal ? boxCollider.size.x : boxCollider.size.y;
        return length * Mathf.Max(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public Vector3 GetPipeCenter()
    {
        // Menghitung pusat collider dengan mempertimbangkan skala objek
        return boxCollider.bounds.center;
    }
}

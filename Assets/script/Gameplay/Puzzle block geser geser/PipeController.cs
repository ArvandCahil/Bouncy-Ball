using UnityEngine;

public class PipeController : MonoBehaviour
{
    [SerializeField] private bool isHorizontal;
    [SerializeField] private float manualLength; // Panjang manual yang dapat disesuaikan
    [SerializeField] private Vector3 centerOffset; // Offset untuk menentukan pusat area gerakan

    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public bool IsHorizontal => isHorizontal;

    public float GetPipeLength()
    {
        // Menggunakan panjang manual jika disetel
        return manualLength;
    }

    public Vector3 GetPipeCenter()
    {
        // Menyesuaikan pusat berdasarkan pivot yang ada di tepi dan offset
        Vector3 pipeCenter = transform.position + centerOffset;

        // Mengatur pusat berdasarkan orientasi pipa
        if (isHorizontal)
        {
            // Pusat di tengah panjang pipa jika horizontal
            pipeCenter += new Vector3(GetPipeLength() / 2, 0, 0);
        }
        else
        {
            // Pusat di tengah panjang pipa jika vertikal
            pipeCenter += new Vector3(0, GetPipeLength() / 2, 0);
        }

        return pipeCenter;
    }
}

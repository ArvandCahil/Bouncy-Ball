using UnityEngine;

public class PipeController : MonoBehaviour
{
    [SerializeField] private bool isHorizontalX; // Checkbox untuk orientasi horizontal pada sumbu X
    [SerializeField] private bool isHorizontalZ; // Checkbox untuk orientasi horizontal pada sumbu Z
    [SerializeField] private bool isVertical; // Checkbox untuk orientasi vertikal

    [SerializeField] private float manualLength = 10f; // Panjang manual pipa
    [SerializeField] private Vector3 centerOffset = Vector3.zero; // Offset pusat pipa

    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public bool IsHorizontalX => isHorizontalX; // Getter untuk orientasi horizontal sumbu X
    public bool IsHorizontalZ => isHorizontalZ; // Getter untuk orientasi horizontal sumbu Z
    public bool IsVertical => isVertical; // Getter untuk orientasi vertikal

    public float GetPipeLength()
    {
        return manualLength; // Menggunakan panjang manual pipa
    }

    public Vector3 GetPipeCenter()
    {
        // Menyesuaikan pusat berdasarkan pivot yang ada di tepi dan offset
        Vector3 pipeCenter = transform.position + centerOffset;

        // Mengatur pusat berdasarkan orientasi pipa
        if (isHorizontalX)
        {
            pipeCenter += new Vector3(GetPipeLength() / 2, 0, 0); // Pusat di tengah panjang pipa jika horizontal X
        }
        else if (isHorizontalZ)
        {
            pipeCenter += new Vector3(0, 0, GetPipeLength() / 2); // Pusat di tengah panjang pipa jika horizontal Z
        }
        else if (isVertical)
        {
            pipeCenter += new Vector3(0, GetPipeLength() / 2, 0); // Pusat di tengah panjang pipa jika vertikal
        }

        return pipeCenter;
    }
}

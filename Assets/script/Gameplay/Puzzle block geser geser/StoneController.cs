using UnityEngine;

public class StoneController : MonoBehaviour
{
    [SerializeField] private Material hoverMaterial; // Material untuk efek hover
    [SerializeField] private Material defaultMaterial; // Material default

    private Vector3 offset;
    private bool isDragging = false;
    private PipeController currentPipe;
    private Renderer stoneRenderer;

    private void Awake()
    {
        stoneRenderer = GetComponent<Renderer>();
        if (stoneRenderer != null)
        {
            // Simpan material default pada saat awal
            stoneRenderer.material = defaultMaterial;
        }
    }

    private void OnMouseEnter()
    {
        if (!isDragging)
        {
            // Ubah material menjadi hoverMaterial saat kursor berada di atas stone
            if (stoneRenderer != null && hoverMaterial != null)
            {
                stoneRenderer.material = hoverMaterial;
            }
        }
    }

    private void OnMouseExit()
    {
        if (!isDragging)
        {
            // Kembalikan material ke defaultMaterial saat kursor keluar dari stone
            if (stoneRenderer != null && defaultMaterial != null)
            {
                stoneRenderer.material = defaultMaterial;
            }
        }
    }

    private void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;
        currentPipe = GetComponentInParent<PipeController>();
        // Pastikan efek hover tetap aktif saat mulai drag
        if (stoneRenderer != null && hoverMaterial != null)
        {
            stoneRenderer.material = hoverMaterial;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
        // Nonaktifkan efek hover saat tidak lagi di-drag
        if (stoneRenderer != null && defaultMaterial != null)
        {
            stoneRenderer.material = defaultMaterial;
        }
    }

    private void Update()
    {
        if (isDragging && currentPipe != null)
        {
            Vector3 mousePosition = GetMouseWorldPosition() + offset;
            Vector3 pipeCenter = currentPipe.GetPipeCenter();
            float pipeLength = currentPipe.GetPipeLength() / 2;

            Debug.Log($"Pipe Center: {pipeCenter}, Pipe Length: {pipeLength}, Mouse Position: {mousePosition}");

            if (currentPipe.IsHorizontal)
            {
                // Batasi gerakan sepanjang pipa berdasarkan pusat collider manual
                float clampedX = Mathf.Clamp(mousePosition.x, pipeCenter.x - pipeLength, pipeCenter.x + pipeLength);
                transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
            }
            else
            {
                // Batasi gerakan sepanjang pipa berdasarkan pusat collider manual
                float clampedY = Mathf.Clamp(mousePosition.y, pipeCenter.y - pipeLength, pipeCenter.y + pipeLength);
                transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}

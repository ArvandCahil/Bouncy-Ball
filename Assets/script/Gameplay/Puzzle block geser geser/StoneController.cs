using UnityEngine;

public class StoneController : MonoBehaviour
{
    [SerializeField] private Material hoverMaterial; // Material saat hover
    [SerializeField] private Material defaultMaterial; // Material default
    [SerializeField] private CameraControl cameraControl; // Referensi ke script CameraControl

    [SerializeField] private float moveSpeed = 5f; // Kecepatan pergerakan stone yang dapat diatur dari Inspector

    private Vector3 offset; // Offset posisi saat drag
    private bool isDragging = false; // Status apakah stone sedang di-drag
    private PipeController currentPipe; // Referensi ke PipeController
    private Renderer stoneRenderer; // Renderer untuk stone
    private Vector3 targetPosition; // Posisi target untuk interpolasi
    private bool isMoving = false; // Status apakah stone sedang bergerak

    private void Awake()
    {
        stoneRenderer = GetComponent<Renderer>();
        if (stoneRenderer != null)
        {
            stoneRenderer.material = defaultMaterial; // Set material default pada awalnya
        }
    }

    private void OnMouseEnter()
    {
        if (cameraControl != null && !isDragging)
        {
            cameraControl.SetCameraActive(false); // Nonaktifkan mode kamera saat mouse masuk ke stone dan tidak sedang di-drag
        }

        if (stoneRenderer != null && hoverMaterial != null)
        {
            stoneRenderer.material = hoverMaterial; // Ganti material menjadi hover saat mouse masuk
        }
    }

    private void OnMouseExit()
    {
        if (!isDragging && cameraControl != null)
        {
            cameraControl.SetCameraActive(true); // Aktifkan kembali mode kamera jika tidak ada dragging
        }

        if (!isDragging && stoneRenderer != null && defaultMaterial != null)
        {
            stoneRenderer.material = defaultMaterial; // Kembali ke material default saat mouse keluar
        }
    }

    private void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPosition(); // Hitung offset saat mulai dragging
        isDragging = true;
        currentPipe = GetComponentInParent<PipeController>(); // Ambil referensi ke PipeController saat drag dimulai

        if (stoneRenderer != null && hoverMaterial != null)
        {
            stoneRenderer.material = hoverMaterial; // Ganti material menjadi hover saat mouse klik
        }

        if (cameraControl != null)
        {
            cameraControl.SetCameraActive(false); // Nonaktifkan mode kamera saat dragging dimulai
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;

        if (cameraControl != null)
        {
            cameraControl.SetCameraActive(true); // Aktifkan kembali mode kamera saat selesai dragging
        }

        if (stoneRenderer != null && defaultMaterial != null)
        {
            stoneRenderer.material = defaultMaterial; // Kembali ke material default saat mouse dilepas
        }
    }

    private void Update()
    {
        if (isDragging && currentPipe != null)
        {
            Vector3 mousePosition = GetMouseWorldPosition() + offset; // Hitung posisi mouse dunia dengan offset
            Vector3 pipeCenter = currentPipe.GetPipeCenter(); // Ambil pusat pipa
            float pipeLength = currentPipe.GetPipeLength() / 2; // Hitung panjang setengah pipa

            Vector3 newPosition;
            if (currentPipe.IsHorizontalX)
            {
                float clampedX = Mathf.Clamp(mousePosition.x, pipeCenter.x - pipeLength, pipeCenter.x + pipeLength);
                newPosition = new Vector3(clampedX, transform.position.y, transform.position.z); // Posisi baru horizontal X
            }
            else if (currentPipe.IsHorizontalZ)
            {
                float clampedZ = Mathf.Clamp(mousePosition.z, pipeCenter.z - pipeLength, pipeCenter.z + pipeLength);
                newPosition = new Vector3(transform.position.x, transform.position.y, clampedZ); // Posisi baru horizontal Z
            }
            else if (currentPipe.IsVertical)
            {
                float clampedY = Mathf.Clamp(mousePosition.y, pipeCenter.y - pipeLength, pipeCenter.y + pipeLength);
                newPosition = new Vector3(transform.position.x, clampedY, transform.position.z); // Posisi baru vertikal
            }
            else
            {
                return; // Jika tidak ada orientasi yang valid, keluar dari fungsi
            }

            // Set target position untuk interpolasi
            targetPosition = newPosition;
            isMoving = true;
        }

        if (isMoving)
        {
            // Gerakkan stone secara halus menuju posisi target
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

            // Hentikan pergerakan jika sudah cukup dekat dengan target
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }
    }

    // Mengambil posisi mouse dalam dunia
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}

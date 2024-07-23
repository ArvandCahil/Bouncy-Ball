using UnityEngine;

public class StoneController : MonoBehaviour
{
    [SerializeField] private Material hoverMaterial; // Material untuk efek hover
    [SerializeField] private Material defaultMaterial; // Material default
    [SerializeField] private CameraControl cameraControl; // Referensi ke kontroler kamera

    private Vector3 offset; // Offset posisi batu
    private bool isDragging = false; // Status apakah batu sedang diseret
    private PipeController currentPipe; // Referensi ke kontroler pipa
    private Renderer stoneRenderer; // Renderer batu

    private void Awake()
    {
        stoneRenderer = GetComponent<Renderer>();
        if (stoneRenderer != null)
        {
            stoneRenderer.material = defaultMaterial; // Set material default saat inisialisasi
        }
    }

    private void OnMouseEnter()
    {
        // Jika mouse memasuki area batu
        if (cameraControl != null)
        {
            cameraControl.SetCameraActive(false); // Nonaktifkan kamera saat hover
        }

        if (stoneRenderer != null && hoverMaterial != null)
        {
            stoneRenderer.material = hoverMaterial; // Ganti material menjadi hoverMaterial
        }
    }

    private void OnMouseExit()
    {
        // Jika mouse keluar dari area batu
        if (!isDragging && cameraControl != null)
        {
            cameraControl.SetCameraActive(true); // Aktifkan kamera saat tidak diseret
        }

        if (!isDragging && stoneRenderer != null && defaultMaterial != null)
        {
            stoneRenderer.material = defaultMaterial; // Kembalikan material ke defaultMaterial
        }
    }

    private void OnMouseDown()
    {
        // Saat mouse ditekan pada batu
        offset = transform.position - GetMouseWorldPosition(); // Hitung offset posisi batu
        isDragging = true; // Set status sedang diseret
        currentPipe = GetComponentInParent<PipeController>(); // Dapatkan referensi ke PipeController

        if (stoneRenderer != null && hoverMaterial != null)
        {
            stoneRenderer.material = hoverMaterial; // Ganti material menjadi hoverMaterial saat diseret
        }
    }

    private void OnMouseUp()
    {
        // Saat mouse dilepas dari batu
        isDragging = false;

        if (cameraControl != null)
        {
            cameraControl.SetCameraActive(true); // Aktifkan kamera saat tidak diseret
        }

        if (stoneRenderer != null && defaultMaterial != null)
        {
            stoneRenderer.material = defaultMaterial; // Kembalikan material ke defaultMaterial
        }
    }

    private void Update()
    {
        // Perbarui posisi batu saat diseret
        if (isDragging && currentPipe != null)
        {
            Vector3 mousePosition = GetMouseWorldPosition() + offset; // Dapatkan posisi mouse di dunia
            Vector3 pipeCenter = currentPipe.GetPipeCenter(); // Dapatkan pusat pipa
            float pipeLength = currentPipe.GetPipeLength() / 2; // Hitung panjang setengah pipa

            if (currentPipe.IsHorizontal)
            {
                // Jika pipa horizontal, batasi pergerakan batu pada sumbu X
                mousePosition.y = transform.position.y;
                mousePosition.z = transform.position.z;
                mousePosition.x = Mathf.Clamp(mousePosition.x, pipeCenter.x - pipeLength, pipeCenter.x + pipeLength);
            }
            else
            {
                // Jika pipa vertikal, batasi pergerakan batu pada sumbu Z
                mousePosition.x = transform.position.x;
                mousePosition.y = transform.position.y;
                mousePosition.z = Mathf.Clamp(mousePosition.z, pipeCenter.z - pipeLength, pipeCenter.z + pipeLength);
            }

            transform.position = mousePosition; // Set posisi batu
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Mengubah posisi mouse menjadi posisi dunia
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
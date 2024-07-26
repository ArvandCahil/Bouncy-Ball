using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minFOV = 20f;
    [SerializeField] private float maxFOV = 60f;
    [SerializeField] private float mouseMoveThreshold = 1f;
    [SerializeField] private float cameraSensitivity = 300f;
    [SerializeField] private StoneController stoneController;

    public bool isCameraModeActive { get; private set; } = false;

    private bool isCameraMode = false;
    private Vector3 lastMousePosition;

    void Update()
    {
        if (freeLookCamera.enabled)
        {
            HandleZoom();
            HandleTouchInput();

            if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
            {
                lastMousePosition = Input.mousePosition;
                isCameraMode = false;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

                if (mouseDelta.magnitude > mouseMoveThreshold)
                {
                    isCameraMode = true;
                }

                if (isCameraMode)
                {
                    HandleCameraMovement(mouseDelta);
                }
            }
            else
            {
                if (isCameraMode)
                {
                    EndCameraMode();
                }

                // Nonaktifkan pergerakan kamera saat mode kamera selesai
                freeLookCamera.m_XAxis.m_MaxSpeed = 0;
                freeLookCamera.m_YAxis.m_MaxSpeed = 0;
            }

            lastMousePosition = Input.mousePosition;
        }
    }

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            float currentFOV = freeLookCamera.m_Lens.FieldOfView;
            float newFOV = Mathf.Clamp(currentFOV - scrollInput * zoomSpeed, minFOV, maxFOV);
            freeLookCamera.m_Lens.FieldOfView = newFOV;
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            float currentFOV = freeLookCamera.m_Lens.FieldOfView;
            float newFOV = Mathf.Clamp(currentFOV + deltaMagnitudeDiff * zoomSpeed, minFOV, maxFOV);
            freeLookCamera.m_Lens.FieldOfView = newFOV;
        }
    }

    private void HandleCameraMovement(Vector3 mouseDelta)
    {
        freeLookCamera.m_XAxis.Value += mouseDelta.x * cameraSensitivity * Time.deltaTime;
        freeLookCamera.m_YAxis.Value -= mouseDelta.y * cameraSensitivity * Time.deltaTime / 150f;
        isCameraModeActive = true;

        // Sembunyikan cursor saat mode kamera aktif
        Cursor.visible = false;

        // Nonaktifkan StoneController dan collider-nya saat mode kamera aktif
        if (stoneController != null)
        {
            stoneController.enabled = false;
            Collider stoneCollider = stoneController.GetComponent<Collider>();
            if (stoneCollider != null)
            {
                stoneCollider.enabled = false;
            }
        }
    }

    private void EndCameraMode()
    {
        isCameraModeActive = false;

        // Tampilkan cursor saat mode kamera selesai
        Cursor.visible = true;

        // Aktifkan StoneController dan collider-nya
        if (stoneController != null)
        {
            stoneController.enabled = true;
            Collider stoneCollider = stoneController.GetComponent<Collider>();
            if (stoneCollider != null)
            {
                stoneCollider.enabled = true;
            }
        }
    }

    public void SetCameraActive(bool active)
    {
        freeLookCamera.enabled = active;
        if (active)
        {
            // Sembunyikan cursor saat kamera aktif
            Cursor.visible = false;
            if (stoneController != null)
            {
                stoneController.enabled = false;
                Collider stoneCollider = stoneController.GetComponent<Collider>();
                if (stoneCollider != null)
                {
                    stoneCollider.enabled = false;
                }
            }
        }
        else
        {
            // Tampilkan cursor saat kamera tidak aktif
            Cursor.visible = true;
            if (stoneController != null)
            {
                stoneController.enabled = true;
                Collider stoneCollider = stoneController.GetComponent<Collider>();
                if (stoneCollider != null)
                {
                    stoneCollider.enabled = true;
                }
            }
        }
    }
}

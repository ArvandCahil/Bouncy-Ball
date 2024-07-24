using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private bool isCameraModeActive = false;
    [SerializeField] private StoneController stoneController;

    private bool isCameraMode = false;
    private Vector3 lastMousePosition;

    void Update()
    {
        // Pastikan freeLookCamera diaktifkan sebelum melakukan zoom
        if (freeLookCamera.enabled)
        {
            // Zoom in dan zoom out dengan scroll wheel (PC/laptop)
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0f)
            {
                float currentFOV = freeLookCamera.m_Lens.FieldOfView;
                float newFOV = Mathf.Clamp(currentFOV - scrollInput * zoomSpeed, minFOV, maxFOV);
                freeLookCamera.m_Lens.FieldOfView = newFOV;
            }

            // Zoom in dan zoom out dengan pinch gesture (Android)
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

            // Deteksi klik dan pergerakan mouse untuk mode kamera
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
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    Cursor.SetCursor(null, new Vector2(-10000, -10000), CursorMode.ForceSoftware);

                    freeLookCamera.m_XAxis.m_MaxSpeed = cameraSensitivity;
                    freeLookCamera.m_YAxis.m_MaxSpeed = cameraSensitivity / 150f;
                    isCameraModeActive = true;

                    // Nonaktifkan StoneController
                    if (stoneController != null)
                    {
                        stoneController.enabled = false;

                        // Nonaktifkan collider jika ada
                        Collider stoneCollider = stoneController.GetComponent<Collider>();
                        if (stoneCollider != null)
                        {
                            stoneCollider.enabled = false;
                        }
                    }
                }
            }
            else
            {
                if (isCameraMode)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    isCameraModeActive = false;

                    // Aktifkan StoneController
                    if (stoneController != null)
                    {
                        stoneController.enabled = true;

                        // Aktifkan collider jika ada
                        Collider stoneCollider = stoneController.GetComponent<Collider>();
                        if (stoneCollider != null)
                        {
                            stoneCollider.enabled = true;
                        }
                    }
                }

                freeLookCamera.m_XAxis.m_MaxSpeed = 0;
                freeLookCamera.m_YAxis.m_MaxSpeed = 0;
            }

            lastMousePosition = Input.mousePosition;
        }
    }

    public void SetCameraActive(bool active)
    {
        freeLookCamera.enabled = active;
        if (active)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Cursor.SetCursor(null, new Vector2(-10000, -10000), CursorMode.ForceSoftware);

            // Nonaktifkan StoneController
            if (stoneController != null)
            {
                stoneController.enabled = false;

                // Nonaktifkan collider jika ada
                Collider stoneCollider = stoneController.GetComponent<Collider>();
                if (stoneCollider != null)
                {
                    stoneCollider.enabled = false;
                }
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            // Aktifkan StoneController
            if (stoneController != null)
            {
                stoneController.enabled = true;

                // Aktifkan collider jika ada
                Collider stoneCollider = stoneController.GetComponent<Collider>();
                if (stoneCollider != null)
                {
                    stoneCollider.enabled = true;
                }
            }
        }
    }
}

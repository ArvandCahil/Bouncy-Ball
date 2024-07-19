using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    public float zoomSpeed = 2f;
    public float minFOV = 20f;
    public float maxFOV = 60f;
    public float mouseMoveThreshold = 1f; // Threshold untuk mendeteksi pergerakan mouse
    public float cameraSensitivity = 300f; // Sensitivitas kamera

    [SerializeField]
    private bool isCameraModeActive = false; // Checkbox untuk mode kamera aktif (disertakan di Inspector)

    private bool isCameraMode = false;
    private Vector3 lastMousePosition;

    void Update()
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

        // Aktifkan atau nonaktifkan kontrol kamera berdasarkan input mouse (PC/laptop) atau touch (Android)
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            // Simpan posisi mouse saat ini
            lastMousePosition = Input.mousePosition;
            isCameraMode = false;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

            // Jika pergerakan mouse melebihi threshold, aktifkan mode kamera
            if (mouseDelta.magnitude > mouseMoveThreshold)
            {
                isCameraMode = true;
            }

            if (isCameraMode)
            {
                // Sembunyikan kursor saat tombol mouse kiri ditekan dan ada pergerakan mouse (PC/laptop)
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                // Gerakkan kamera
                freeLookCamera.m_XAxis.m_MaxSpeed = cameraSensitivity;  // Gunakan sensitivitas kamera yang diatur
                freeLookCamera.m_YAxis.m_MaxSpeed = cameraSensitivity / 150f;  // Sesuaikan kecepatan rotasi sesuai kebutuhanmu
                isCameraModeActive = true; // Centang checkbox saat mode kamera aktif
            }
        }
        else
        {
            if (isCameraMode)
            {
                // Tampilkan kursor saat tombol mouse kiri dilepas (PC/laptop)
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isCameraModeActive = false; // Uncentang checkbox saat mode kamera tidak aktif
            }

            // Nonaktifkan gerakan kamera
            freeLookCamera.m_XAxis.m_MaxSpeed = 0;
            freeLookCamera.m_YAxis.m_MaxSpeed = 0;
        }

        lastMousePosition = Input.mousePosition;
    }
}

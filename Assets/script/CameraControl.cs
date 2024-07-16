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
    public KeyCode activateCameraKey = KeyCode.LeftShift;

    void Update()
    {
        // Zoom in dan zoom out dengan scroll wheel
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            float currentFOV = freeLookCamera.m_Lens.FieldOfView;
            float newFOV = Mathf.Clamp(currentFOV - scrollInput * zoomSpeed, minFOV, maxFOV);
            freeLookCamera.m_Lens.FieldOfView = newFOV;
        }

        // Aktifkan atau nonaktifkan kontrol kamera dengan tombol shift kiri
        if (Input.GetKey(activateCameraKey))
        {
            // Sembunyikan kursor dan gerakkan kamera
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            freeLookCamera.m_XAxis.m_MaxSpeed = 300;  // Sesuaikan kecepatan rotasi sesuai kebutuhanmu
            freeLookCamera.m_YAxis.m_MaxSpeed = 2;
        }
        else
        {
            // Tampilkan kursor dan nonaktifkan gerakan kamera
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            freeLookCamera.m_XAxis.m_MaxSpeed = 0;
            freeLookCamera.m_YAxis.m_MaxSpeed = 0;
        }
    }
}


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

    private bool isCameraMode = false;
    private Vector3 lastMousePosition;

    void Update()
    {
        if (freeLookCamera.enabled)
        {
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

                    freeLookCamera.m_XAxis.m_MaxSpeed = cameraSensitivity;
                    freeLookCamera.m_YAxis.m_MaxSpeed = cameraSensitivity / 150f;
                    isCameraModeActive = true;
                }
            }
            else
            {
                if (isCameraMode)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    isCameraModeActive = false;
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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    public float zoomSpeed = 2f;
    public float minFOV = 20f;
    public float maxFOV = 60f;

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0f)
        {
            float currentFOV = freeLookCamera.m_Lens.FieldOfView;

            float newFOV = Mathf.Clamp(currentFOV - scrollInput * zoomSpeed, minFOV, maxFOV);

            freeLookCamera.m_Lens.FieldOfView = newFOV;
        }
    }
}


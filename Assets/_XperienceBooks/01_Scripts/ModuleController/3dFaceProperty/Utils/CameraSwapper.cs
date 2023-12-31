﻿
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class CameraSwapper : MonoBehaviour
{


    [SerializeField]
    private Button swapCameraToggle;


    /// <summary>
    /// The camera manager for switching the camera direction.
    /// </summary>
    public ARCameraManager cameraManager
    {
        get => m_CameraManager;
        set => m_CameraManager = value;
    }

    [SerializeField]
    ARCameraManager m_CameraManager;


    private void Awake()
    {
        swapCameraToggle.onClick.AddListener(OnSwapCameraButtonPress);
    }


    /// <summary>
    /// On button press callback to toggle the requested camera facing direction.
    /// </summary>
    public void OnSwapCameraButtonPress()
    {
        Debug.Assert(m_CameraManager != null, "camera manager cannot be null");
        CameraFacingDirection newFacingDirection;
        switch (m_CameraManager.requestedFacingDirection)
        {
            case CameraFacingDirection.World:
                newFacingDirection = CameraFacingDirection.User;
                break;
            case CameraFacingDirection.User:
            default:
                newFacingDirection = CameraFacingDirection.World;
                break;
        }

        Debug.Log($"Switching ARCameraManager.requestedFacingDirection from {m_CameraManager.requestedFacingDirection} to {newFacingDirection}");
        m_CameraManager.requestedFacingDirection = newFacingDirection;
    }
}

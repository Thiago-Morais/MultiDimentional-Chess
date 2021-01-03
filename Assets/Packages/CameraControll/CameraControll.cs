using UnityEngine;
using Cinemachine;
using System;

public class CameraControll : MonoBehaviour, IInitializable
{
    #region -------- FIELDS
    public CinemachineVirtualCameraBase hoverCamera;
    #endregion //FIELDS

    #region -------- PROPERTIES
    #endregion //PROPERTIES

    #region -------- METHODS
    [ContextMenu(nameof(ActivateHoverCamera))]
    public void ActivateHoverCamera() => hoverCamera.gameObject.SetActive(true);
    [ContextMenu(nameof(DeactivateHoverCamera))]
    public void DeactivateHoverCamera() => hoverCamera.gameObject.SetActive(false);
    public IInitializable Initialized(Transform parent = null)
    {
        InitializeVariables();
        return this;
    }
    public void InitializeVariables()
    {
        if (!hoverCamera) hoverCamera = GetComponentInChildren<CinemachineVirtualCameraBase>();
        if (!hoverCamera)
        {
            hoverCamera = new GameObject(nameof(hoverCamera)).AddComponent<CinemachineVirtualCamera>();
            hoverCamera.transform.SetParent(transform);
        }
    }
    #endregion //METHODS
}
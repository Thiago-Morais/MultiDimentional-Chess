using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.InputSystem;

public class HoverControll : MonoBehaviour, IInitializable
{
    #region -------- FIELDS
    public CinemachineFreeLook hoverCamera;
    public string cachedXAxis = "Mouse X";
    public string cachedYAxis = "Mouse Y";
    public float[] cachedHeights;
    public float[] cachedRadiuses;
    public float zoom;
    public float zoomMultiplier = 0.01f;
    public float minZoom = .1f;
    public CinemachineFreeLook.Orbit[] minOrbit = new CinemachineFreeLook.Orbit[3];
    #endregion //FIELDS

    #region -------- PROPERTIES
    #endregion //PROPERTIES

    void Awake()
    {
        InitializeVariables();
        CacheVCamInputAxis();
        CacheFreeLookOrbits();
    }
    void Start() => DeactivateHoverCamera();
    #region -------- METHODS
    #region -------- INPUTS
    public void OnHover(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed: ActivateHoverCamera(); break;
            case InputActionPhase.Canceled: DeactivateHoverCamera(); break;
        }
    }
    public void OnZoom(InputAction.CallbackContext context)
    {
        Debug.Log($"{nameof(context.phase)} = {context.phase}", gameObject);
        switch (context.phase)
        {
            case InputActionPhase.Performed: zoom = context.ReadValue<float>(); break;
        }
        SetZoom(zoom);
    }
    #endregion //INPUTS
    [ContextMenu(nameof(ActivateHoverCamera))] public void ActivateHoverCamera() => ResetFreeLookAxis();
    [ContextMenu(nameof(DeactivateHoverCamera))] public void DeactivateHoverCamera() => RemoveFreeLookAxis();
    public IInitializable Initialized(Transform parent = null)
    {
        InitializeVariables();
        return this;
    }
    public void InitializeVariables()
    {
        if (!hoverCamera) hoverCamera = GetComponentInChildren<CinemachineFreeLook>();
        if (!hoverCamera)
        {
            hoverCamera = new GameObject(nameof(hoverCamera)).AddComponent<CinemachineFreeLook>();
            hoverCamera.transform.SetParent(transform);
        }
    }
    public void CacheVCamInputAxis()
    {
        cachedXAxis = hoverCamera.m_XAxis.m_InputAxisName;
        cachedYAxis = hoverCamera.m_YAxis.m_InputAxisName;
    }
    public void CacheFreeLookOrbits()
    {
        cachedHeights = cachedRadiuses = new float[hoverCamera.m_Orbits.Length];
        for (int i = 0; i < cachedHeights.Length; i++)
        {
            cachedHeights[i] = hoverCamera.m_Orbits[i].m_Height;
            cachedRadiuses[i] = hoverCamera.m_Orbits[i].m_Radius;
        }
    }
    public void ResetFreeLookAxis()
    {
        hoverCamera.m_XAxis.m_InputAxisName = cachedXAxis;
        hoverCamera.m_YAxis.m_InputAxisName = cachedYAxis;
    }
    public void RemoveFreeLookAxis()
    {
        hoverCamera.m_XAxis.m_InputAxisName = "";
        hoverCamera.m_YAxis.m_InputAxisName = "";
        hoverCamera.m_XAxis.m_InputAxisValue = 0;
        hoverCamera.m_YAxis.m_InputAxisValue = 0;
    }
    // public void SetZoom(float scroll)
    // {
    //     for (int i = 0; i < hoverCamera.m_Orbits.Length; i++)
    //     {
    //         hoverCamera.m_Orbits[i].m_Height = Mathf.Clamp(hoverCamera.m_Orbits[i].m_Height - scroll * zoomMultiplier, minZoom, Mathf.Infinity);
    //         hoverCamera.m_Orbits[i].m_Radius = Mathf.Clamp(hoverCamera.m_Orbits[i].m_Radius - scroll * zoomMultiplier, minZoom, Mathf.Infinity);
    //     }
    // }
    public void SetZoom(float scroll)
    {
        if (scroll == 0) return;
        for (int i = 0; i < hoverCamera.m_Orbits.Length; i++)
        {
            float v = -scroll * zoomMultiplier;
            hoverCamera.m_Orbits[i].m_Height += Math.Sign(hoverCamera.m_Orbits[i].m_Height) * v;
            hoverCamera.m_Orbits[i].m_Radius += Math.Sign(hoverCamera.m_Orbits[i].m_Radius) * v;
        }
    }
    // public void SetZoom(float scroll)
    // {
    //     for (int i = 0; i < hoverCamera.m_Orbits.Length; i++)
    //     {
    //         // if (hoverCamera.m_Orbits[i].m_Height - NewMethod(scroll, hoverCamera.m_Orbits[i].m_Height) >= minOrbit[i].m_Height ||
    //         // hoverCamera.m_Orbits[i].m_Radius - NewMethod(scroll, hoverCamera.m_Orbits[i].m_Radius) >= minOrbit[i].m_Radius)
    //         //     continue;
    //         hoverCamera.m_Orbits[i].m_Height -= NewMethod(scroll, hoverCamera.m_Orbits[i].m_Height);
    //         hoverCamera.m_Orbits[i].m_Radius -= NewMethod(scroll, hoverCamera.m_Orbits[i].m_Radius);
    //     }
    // }

    private float NewMethod(float scroll, float value)
    {
        return Mathf.Sign(value) * scroll * zoomMultiplier;
    }
    #endregion //METHODS
}
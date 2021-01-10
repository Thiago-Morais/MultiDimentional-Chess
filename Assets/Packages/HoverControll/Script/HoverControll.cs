using UnityEngine;
using Cinemachine;
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
    #endregion //FIELDS

    #region -------- PROPERTIES
    #endregion //PROPERTIES

    #region -------- OUTSIDE CALL
    void Awake()
    {
        InitializeVariables();
        CacheVCamInputAxis();
        CacheFreeLookOrbits();
    }
    void Start() => DeactivateHoverCamera();
    #endregion //OUTSIDE CALL

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
        AddZoom(zoom);
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
    public void AddZoom(float scroll)
    {
        if (scroll == 0) return;
        SetZoom(-scroll * zoomMultiplier);
    }
    public void SetZoom(float scaledScroll)
    {
        if (DropBelowMinimum(scaledScroll))
        {
            SetOrbits(minZoom);
            return;
        }
        Vector2 orbitVector = new Vector2();
        for (int i = 0; i < hoverCamera.m_Orbits.Length; i++)
        {
            orbitVector.Set(hoverCamera.m_Orbits[i].m_Height, hoverCamera.m_Orbits[i].m_Radius);
            orbitVector = orbitVector.normalized * scaledScroll;
            hoverCamera.m_Orbits[i].m_Height += orbitVector.x;
            hoverCamera.m_Orbits[i].m_Radius += orbitVector.y;
        }
    }
    bool DropBelowMinimum(float scaledScroll)
    {
        Vector2 orbitVector = new Vector2();
        for (int i = 0; i < hoverCamera.m_Orbits.Length; i++)
        {
            orbitVector.Set(hoverCamera.m_Orbits[i].m_Height, hoverCamera.m_Orbits[i].m_Radius);
            orbitVector = orbitVector.normalized * scaledScroll;
            orbitVector.x += hoverCamera.m_Orbits[i].m_Height;
            orbitVector.y += hoverCamera.m_Orbits[i].m_Radius;
            if (orbitVector.magnitude < minZoom) return true;
        }
        return false;
    }
    void SetOrbits(float magnitude)
    {
        for (int i = 0; i < hoverCamera.m_Orbits.Length; i++)
        {
            Vector2 orbitVector = new Vector2(hoverCamera.m_Orbits[i].m_Height, hoverCamera.m_Orbits[i].m_Radius);
            orbitVector = orbitVector.normalized * magnitude;
            hoverCamera.m_Orbits[i].m_Height = orbitVector.x;
            hoverCamera.m_Orbits[i].m_Radius = orbitVector.y;
        }
    }

    #endregion //METHODS
}
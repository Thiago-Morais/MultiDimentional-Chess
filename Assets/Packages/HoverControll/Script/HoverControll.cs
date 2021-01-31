using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using System;

public class HoverControll : MonoBehaviour, IInitializable
{
    #region -------- FIELDS
    public CinemachineFreeLook hoverCamera;
    public string initialXAxisName = "Mouse X";
    public string initialYAxisName = "Mouse Y";
    public float[] initialHeights;
    public float[] initialRadiuses;
    public float scroll;
    [SerializeField] float hoverSensitivity = 1;
    public float zoomMultiplier = 0.01f;
    public float minZoom = .1f;
    public Vector2 inicialHoverSpeed;
    public Vector2 secondTouchPosition;
    Vector2 secondTouchPositionCached;
    Vector2 firstPointStart;
    Vector2 secondPointStart;
    Vector2 firstPointPosition;
    Vector2 secondPointPosition;
    Vector2 firstPointDelta;
    Vector2 secondPointDelta;
    Vector2 m_HoverSpeed = new Vector2();
    Vector2 m_InitialHoverSpeed;
    #endregion //FIELDS

    #region -------- PROPERTIES
    public float HoverSensitivity
    {
        get => hoverSensitivity;
        set
        {
            hoverSensitivity = value;
            SetHoverSpeed(m_InitialHoverSpeed * value);
        }
    }
    #endregion //PROPERTIES

    #region -------- OUTSIDE CALL
    void Awake()
    {
        InitializeVariables();
        CacheVCamInputAxis();
        CacheFreeLookOrbits();
        CacheInitialHoverSpeed();

        ApplyHoverSensitivity();
    }
    void Start() => DeactivateHoverCamera();
    #region -------- INPUTS
    public void OnHover(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed: ActivateHoverCamera(); break;
            case InputActionPhase.Canceled: DeactivateHoverCamera(); break;
        }
    }
    public void OnFirstPointPosition(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) firstPointPosition = context.ReadValue<Vector2>();
    }
    public void OnSecondPointPosition(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) secondPointPosition = context.ReadValue<Vector2>();
    }
    public void OnFirstPointDelta(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) firstPointDelta = context.ReadValue<Vector2>();
    }
    public void OnSecondPointDelta(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) secondPointDelta = context.ReadValue<Vector2>();
        TouchZoom();
    }
    public void OnScroll(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            scroll = context.ReadValue<float>();
            AddDeltaZoom(scroll);
        }
    }
    public void TouchZoom()
    {
        float currentDistance = Vector2.Distance(secondPointPosition, firstPointPosition);
        float lastDistance = Vector2.Distance(secondPointPosition - secondPointDelta, firstPointPosition - firstPointDelta);
        float delta = currentDistance - lastDistance;
        Debug.Log(
$@"---------------------ZOOM---------------------
 {nameof(firstPointDelta)} = {firstPointDelta}
 {nameof(secondPointDelta)} = {secondPointDelta}
 {nameof(firstPointPosition)} = {firstPointPosition}
 {nameof(secondPointPosition)} = {secondPointPosition}
 {nameof(currentDistance)} = {currentDistance}
 {nameof(lastDistance)} = {lastDistance}
 {nameof(delta)} = {delta}
---------------------ZOOM---------------------");
        AddDeltaZoom(-delta);
    }
    #endregion //INPUTS
    #endregion //OUTSIDE CALL

    #region -------- METHODS
    #region -------- INTERFACES
    public IInitializable Initialized(Transform parent = null)
    {
        InitializeVariables();
        CacheInitialHoverSpeed();
        return this;
    }
    #endregion //INTERFACES
    [ContextMenu(nameof(ActivateHoverCamera))] public void ActivateHoverCamera() => ResetFreeLookAxis();
    [ContextMenu(nameof(DeactivateHoverCamera))] public void DeactivateHoverCamera() => RemoveFreeLookAxis();
    public void ResetFreeLookAxis()
    {
        hoverCamera.m_XAxis.m_InputAxisName = initialXAxisName;
        hoverCamera.m_YAxis.m_InputAxisName = initialYAxisName;
    }
    public void RemoveFreeLookAxis()
    {
        hoverCamera.m_XAxis.m_InputAxisName = "";
        hoverCamera.m_YAxis.m_InputAxisName = "";
        hoverCamera.m_XAxis.m_InputAxisValue = 0;
        hoverCamera.m_YAxis.m_InputAxisValue = 0;
    }
    #region -------- ZOOM HANDLER
    public void AddDeltaZoom(float delta)
    {
        if (delta == 0) return;
        AddScaledDeltaZoom(delta * zoomMultiplier);
    }
    public void AddScaledDeltaZoom(float scaledDelta)
    {
        if (DropBelowMinimum(scaledDelta))
        {
            SetOrbits(minZoom);
            return;
        }
        Vector2 orbitVector = new Vector2();
        for (int i = 0; i < hoverCamera.m_Orbits.Length; i++)
        {
            orbitVector.Set(hoverCamera.m_Orbits[i].m_Height, hoverCamera.m_Orbits[i].m_Radius);
            orbitVector = orbitVector.normalized * scaledDelta;
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
        Vector2 orbitVector = new Vector2();
        for (int i = 0; i < hoverCamera.m_Orbits.Length; i++)
        {
            orbitVector.Set(hoverCamera.m_Orbits[i].m_Height, hoverCamera.m_Orbits[i].m_Radius);
            orbitVector = orbitVector.normalized * magnitude;
            hoverCamera.m_Orbits[i].m_Height = orbitVector.x;
            hoverCamera.m_Orbits[i].m_Radius = orbitVector.y;
        }
    }
    #endregion //ZOOM HANDLER
    [ContextMenu(nameof(ApplyHoverSensitivity))]
    public void ApplyHoverSensitivity() => SetHoverSpeed(m_InitialHoverSpeed * HoverSensitivity);
    public Vector2 GetHoverSpeed()
    {
        m_HoverSpeed.Set(hoverCamera.m_XAxis.m_MaxSpeed, hoverCamera.m_YAxis.m_MaxSpeed);
        return m_HoverSpeed;
    }
    public void SetHoverSpeed(Vector2 hoverSpeed)
    {
        hoverCamera.m_XAxis.m_MaxSpeed = hoverSpeed.x;
        hoverCamera.m_YAxis.m_MaxSpeed = hoverSpeed.y;
    }
    #region -------- CACHE
    public void CacheVCamInputAxis()
    {
        initialXAxisName = hoverCamera.m_XAxis.m_InputAxisName;
        initialYAxisName = hoverCamera.m_YAxis.m_InputAxisName;
    }
    public void CacheFreeLookOrbits()
    {
        initialHeights = initialRadiuses = new float[hoverCamera.m_Orbits.Length];
        for (int i = 0; i < initialHeights.Length; i++)
        {
            initialHeights[i] = hoverCamera.m_Orbits[i].m_Height;
            initialRadiuses[i] = hoverCamera.m_Orbits[i].m_Radius;
        }
    }
    void CacheInitialHoverSpeed() => m_InitialHoverSpeed.Set(hoverCamera.m_XAxis.m_MaxSpeed, hoverCamera.m_YAxis.m_MaxSpeed);
    #endregion //CACHE
    public void InitializeVariables()
    {
        if (!hoverCamera) hoverCamera = GetComponentInChildren<CinemachineFreeLook>();
        if (!hoverCamera)
        {
            hoverCamera = new GameObject(nameof(hoverCamera)).AddComponent<CinemachineFreeLook>();
            hoverCamera.transform.SetParent(transform);
        }
    }
    #endregion //METHODS
}
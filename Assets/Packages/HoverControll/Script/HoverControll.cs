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
    public float scroll;
    public float zoomMultiplier = 0.01f;
    public float minZoom = .1f;
    public Vector2 secondTouchPosition;
    Vector2 secondTouchPositionCached;
    private Vector2 firstPointStart;
    private Vector2 secondPointStart;
    private Vector2 firstPointPosition;
    private Vector2 secondPointPosition;
    private Vector2 firstPointDelta;
    private Vector2 secondPointDelta;
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
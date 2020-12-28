using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoverHighlight : MonoBehaviour, IHighlighter
{
    #region -------- FIELDS
    public InputChess inputChess;
    public Camera mainCamera;
    [SerializeField] HighlightType highlightType = HighlightType.hover;
    Rigidbody m_CacheRigidBody;
    IHighlightable m_CachedHighlightable;
    Highlight m_CachedHighlight;
    Vector2 m_MousePosition;
    public HighlightType HighlightType { get => highlightType; set => highlightType = value; }
    #endregion //FIELDS

    #region -------- PROPERTIES
    #endregion //PROPERTIES

    #region -------- EXTERNAL CALL
    void Awake() { if (!mainCamera) mainCamera = Camera.main; }
    public void OnPoint(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        m_MousePosition = context.ReadValue<Vector2>();
        UpdateHighlight();
    }
    #endregion //EXTERNAL CALL
    #region -------- METHODS
    void UpdateHighlight()
    {
        Rigidbody currentRigidbody = GetRigidbodyFromRaycast();
        // if (!IsNew(currentRigidbody)) return;

        IHighlightable highlightable = GetHighlightableFrom(currentRigidbody);
        if (!IsNew(highlightable) && IsHover(highlightable)) return;
        // if (IsNew(highlightable)) HoveredOut(m_CachedHighlightable);

        HoveredOut(m_CachedHighlightable);
        HoveredIn(highlightable);

        Cache<IHighlightable>(ref m_CachedHighlightable, highlightable);
        Cache<Rigidbody>(ref m_CacheRigidBody, currentRigidbody);
    }
    Rigidbody GetRigidbodyFromRaycast()
    {
        RaycastHit hit = MouseRayCast(mainCamera);
        return hit.collider?.attachedRigidbody;
    }
    bool IsHover(IHighlightable highlightable) => highlightable?.Highlight.HighlightType == HighlightType;
    public void HoveredIn(IHighlightable hovered) => hovered?.Highlight?.HighlightOn(HighlightType.hover);
    public void HoveredOut(IHighlightable hovered) => hovered?.Highlight?.HighlightUndo();
    RaycastHit MouseRayCast(Camera camera)
    {
        Ray ray = camera.ScreenPointToRay(m_MousePosition);
        RaycastHit[] raycastHits = Physics.RaycastAll(ray);
        return raycastHits.Length > 0 ? raycastHits[0] : default(RaycastHit);
    }
    Highlight GetHighlightFrom(Rigidbody currentRigidbody) => currentRigidbody?.GetComponentInChildren<Highlight>();
    IHighlightable GetHighlightableFrom(Rigidbody currentRigidbody) => currentRigidbody?.GetComponentInChildren<IHighlightable>();
    void Cache<T>(ref T last, T @new) where T : class => last = @new;
    void CacheHighlight(ref Highlight last, Highlight @new)
    {
        m_CachedHighlight?.HighlightUndo();
        last = @new;
    }
    public bool IsNew(Rigidbody @new) => @new != m_CacheRigidBody;
    public bool IsNew(Highlight @new) => @new != m_CachedHighlight;
    public bool IsNew(IHighlightable @new) => @new != m_CachedHighlightable;
    #endregion //METHODS
}

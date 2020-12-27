using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHighlight : MonoBehaviour
{
    #region -------- FIELDS
    public InputChess inputChess;
    public Camera mainCamera;
    Rigidbody m_CacheRigidBody;
    Highlight m_CachedHighlight;
    Vector2 m_MousePosition;
    #endregion //FIELDS

    #region -------- PROPERTIES
    #endregion //PROPERTIES

    #region -------- EXTERNAL CALL
    void Awake() { if (!mainCamera) mainCamera = Camera.main; }
    public void OnPointer(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        m_MousePosition = context.ReadValue<Vector2>();
        UpdateHighlight();
    }

    private void UpdateHighlight()
    {
        UpdateHighlight(Highlight.HighlightType.hover);
    }
    #endregion //EXTERNAL CALL
    #region -------- METHODS
    void UpdateHighlight(Highlight.HighlightType highlightType)
    {
        Rigidbody currentRigidbody = GetRigidbodyFromRaycast();
        if (!IsNew(currentRigidbody)) return;

        Highlight currentHighlightObj = ExtractHighlightFrom(currentRigidbody);
        if (IsNew(currentHighlightObj)) m_CachedHighlight?.HighlightUndo();

        if (currentHighlightObj)
        {
            TrySetHighlightValues(currentHighlightObj, highlightType);
            currentHighlightObj.HighlightOn();
        }

        Cache<Highlight>(ref m_CachedHighlight, currentHighlightObj);
        Cache<Rigidbody>(ref m_CacheRigidBody, currentRigidbody);
    }
    void TrySetHighlightValues(Highlight highlight, Highlight.HighlightType highlightType) => highlight?.TrySetHighlightValues(highlightType);
    Rigidbody GetRigidbodyFromRaycast()
    {
        RaycastHit hit = MouseRayCast(mainCamera);

        Rigidbody currentRigidbody = hit.collider?.attachedRigidbody;
        return currentRigidbody;
    }
    Highlight ExtractHighlightFrom(Rigidbody currentRigidbody) => currentRigidbody?.GetComponentInChildren<Highlight>();
    void ToggleHighlights(Highlight highlight)
    {
        m_CachedHighlight?.HighlightUndo();
    }
    RaycastHit MouseRayCast(Camera camera)
    {
        Ray ray = camera.ScreenPointToRay(m_MousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return hit;
    }
    void Cache<T>(ref T last, T @new) where T : class => last = @new;
    void CacheHighlight(ref Highlight last, Highlight @new)
    {
        m_CachedHighlight?.HighlightUndo();
        last = @new;
    }
    public bool IsNew(Rigidbody @new) => @new != m_CacheRigidBody;
    public bool IsNew(Highlight @new) => @new != m_CachedHighlight;
    #endregion //METHODS
}

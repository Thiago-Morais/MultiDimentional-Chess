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
    Highlight m_CacheHighlight;
    Vector2 m_MousePosition;
    #endregion //FIELDS

    #region -------- PROPERTIES
    #endregion //PROPERTIES

    #region -------- EXTERNAL CALL
    void Awake() { if (!mainCamera) mainCamera = Camera.main; }
    // void Update()
    // {
    //     UpdateHighlight();
    // }
    public void OnPointer(InputAction.CallbackContext context)
    {
        m_MousePosition = context.ReadValue<Vector2>();
        UpdateHighlight(Highlight.HighlightType.hover);
    }
    #endregion //EXTERNAL CALL
    #region -------- METHODS
    void UpdateHighlight(Highlight.HighlightType highlightType)
    {
        Rigidbody currentRigidbody = GetRigidbodyFromRaycast();
        // if (!currentRigidbody) return;
        if (IsCached(currentRigidbody)) return;

        Highlight currentHighlight = ExtractHighlightFrom(currentRigidbody);
        if (currentHighlight)
        {
            TrySetHighlightValues(currentHighlight, highlightType);
            currentHighlight.HighlightOn();
        }

        CacheHighlight(ref m_CacheHighlight, currentHighlight);
        Cache<Rigidbody>(ref m_CacheRigidBody, currentRigidbody);
    }
    // void TrySetHighlightValues(Highlight.HighlightType highlightType) => m_CacheHighlight?.TrySetHighlightValues(highlightType);
    void TrySetHighlightValues(Highlight highlight, Highlight.HighlightType highlightType) => highlight?.TrySetHighlightValues(highlightType);
    Rigidbody GetRigidbodyFromRaycast()
    {
        RaycastHit hit = MouseRayCast(mainCamera);

        Rigidbody currentRigidbody = hit.collider?.attachedRigidbody;
        return currentRigidbody;
    }
    Highlight ExtractHighlightFrom(Rigidbody currentRigidbody)
    {
        Highlight highlight = currentRigidbody?.GetComponentInChildren<Highlight>();
        // if (!highlight) return;

        if (!IsCached(highlight))
        {
            ToggleHighlights(highlight);
        }

        return highlight;
        // Cache<Highlight>(ref m_CacheHighlight, highlight);
    }
    void ToggleHighlights(Highlight highlight)
    {
        m_CacheHighlight?.HighlightOff();
        // highlight?.HighlightOn();
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
        m_CacheHighlight?.HighlightOff();
        last = @new;
    }
    public bool IsCached(Rigidbody @new) => @new == m_CacheRigidBody;
    public bool IsCached(Highlight @new) => @new == m_CacheHighlight;
    #endregion //METHODS
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HighlightHandler : MonoBehaviour
{
    public InputChess inputChess;
    public InputAction mousePosition;
    public Camera mainCamera;
    Rigidbody lastRigidBody;
    Highlight lastHighlight;
    void Awake()
    {
        if (!mainCamera) mainCamera = Camera.main;
    }
    void Update()
    {
        Vector2 mousePosition = this.mousePosition.ReadValue<Vector2>();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        Rigidbody attachedRigidbody = hit.collider?.attachedRigidbody;
        if (attachedRigidbody == lastRigidBody) return;
        if (!attachedRigidbody) { NewRigidBody(attachedRigidbody); return; }

        NewRigidBody(attachedRigidbody);

        Highlight highlight = attachedRigidbody.GetComponentInChildren<Highlight>();
        if (!highlight)
        {
            Debug.Log($"{nameof(highlight)} = {highlight}", gameObject);
            return;
        }

        highlight.SetHighlight(true);
        lastHighlight = highlight;
    }
    void NewRigidBody(Rigidbody attachedRigidbody)
    {
        if (lastHighlight)
        {
            lastHighlight.SetHighlight(false);
            lastHighlight = default;
        }
        lastRigidBody = attachedRigidbody;
    }
    void OnEnable() => mousePosition.Enable();
    void OnDisable() => mousePosition.Disable();
}

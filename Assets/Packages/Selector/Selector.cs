using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Selector : MonoBehaviour
{
    public ISelectable currentSelected;
    [SerializeField] Camera m_Camera;
    [SerializeField] LayerMask selectionMask;
    Vector2 m_PointerPosition;
    void Awake() { if (!m_Camera) m_Camera = Camera.main; }
    public void OnPoint(InputAction.CallbackContext context) => m_PointerPosition = context.ReadValue<Vector2>();
    public void OnSelect(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        ISelectable newSelected = GetSelectableUsingRaycast();
        if (newSelected == null) return;
        ChangeSelection(newSelected);
    }
    void ChangeSelection(ISelectable newSelected)
    {
        currentSelected?.OnDeselected();
        newSelected?.OnSelected();
        currentSelected = newSelected;
        print(currentSelected);
    }
    ISelectable GetSelectableUsingRaycast()
    {
        Ray ray = m_Camera.ScreenPointToRay(m_PointerPosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, float.PositiveInfinity, selectionMask);
        return hit.transform?.GetComponentInChildren<ISelectable>();
    }
}

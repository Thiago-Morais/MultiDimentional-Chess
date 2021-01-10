using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Selector : MonoBehaviour, IMediator<Selector.IntFlags>, IHighlighter
{
    #region -------- FIELDS
    public ISelectable currentSelected;
    [SerializeField] Camera m_Camera;
    [SerializeField] LayerMask selectionMask;
    Vector2 m_PointerPosition;
    ISelectable cachedSelected;
    [SerializeField] HighlightType highlightType = HighlightType.selected;
    #endregion //FIELDS

    #region -------- PROPERTIES
    public HighlightType HighlightType { get => highlightType; set => highlightType = value; }
    public ISelectable CachedSelected { get => cachedSelected; private set => cachedSelected = value; }
    #endregion //PROPERTIES

    #region -------- OUTSIDE CALL
    void Awake()
    {
        SignOn();
        if (!m_Camera) m_Camera = Camera.main;
    }
    public void OnPoint(InputAction.CallbackContext context) { if (context.performed) m_PointerPosition = context.ReadValue<Vector2>(); }
    public void OnSelect(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        UpdateSelect();
    }
    #endregion //OUTSIDE CALL

    #region -------- METHODS
    #region -------- MEDIATOR
    public void SignOn() => ContextMediator.SignOn(this);
    public void Notify(IntFlags intFlag) => ContextMediator.Notify(this, intFlag);
    #endregion //MEDIATOR

    void UpdateSelect()
    {
        ISelectable newSelected = GetSelectableUsingRaycast();

        ChangeSelection(newSelected);
    }
    public void ChangeSelection(ISelectable newSelected)
    {
        if (newSelected != currentSelected)
        {
            Deselect(currentSelected);
            Select(newSelected);
            CachedSelected = currentSelected;
            currentSelected = newSelected;
        }
        else
        {
            Deselect(currentSelected);
            currentSelected = null;
        }
        Notify(IntFlags.SelectionChanged);
        Debug.Log($"{nameof(currentSelected)} = {currentSelected}", gameObject);
    }
    public void Deselect(ISelectable cachedSelected)
    {
        cachedSelected?.OnDeselected();
    }
    void Select(ISelectable currentSelected)
    {
        currentSelected?.Highlight.HighlightOn(HighlightType);
        currentSelected?.OnSelected();
    }
    internal void DeselectAll() => Notify(IntFlags.DeselectAll);
    ISelectable GetSelectableUsingRaycast()
    {
        Ray ray = m_Camera.ScreenPointToRay(m_PointerPosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, float.PositiveInfinity, selectionMask);
        return hit.transform?.GetComponentInChildren<ISelectable>();
    }
    #endregion //METHODS

    #region -------- ENUM
    [Flags]
    public enum IntFlags
    {
        none = 0,
        SelectionChanged = 1 << 0,
        DeselectAll = 1 << 1,
    }
    #endregion //ENUM
}

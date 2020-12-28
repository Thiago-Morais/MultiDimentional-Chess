using System;
using System.Collections;
using System.Collections.Generic;
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
    [Flags]
    public enum IntFlags
    {
        none = 0,
        SelectionChanged = 1 << 0,
        DeselectAll = 1 << 1,
    }
    void Awake()
    {
        SignOn();
        if (!m_Camera) m_Camera = Camera.main;
    }

    #region -------- METHODS
    #region -------- MEDIATOR
    public void SignOn() => ContextMediator.SignOn(this);
    public void Notify(IntFlags intFlag) => ContextMediator.Notify(this, intFlag);
    #endregion //MEDIATOR

    public void OnPoint(InputAction.CallbackContext context) { if (context.performed) m_PointerPosition = context.ReadValue<Vector2>(); }
    public void OnSelect(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        UpdateSelect();
    }
    private void UpdateSelect()
    {
        ISelectable newSelected = GetSelectableUsingRaycast();

        ChangeSelection(newSelected);
    }
    public void ChangeSelection(ISelectable newSelected)
    {
        CachedSelected = currentSelected;
        currentSelected = newSelected;
        Deselect(CachedSelected);
        Select(currentSelected);

        ContextHandler();
        Notify(IntFlags.SelectionChanged);
        Debug.Log($"{nameof(currentSelected)} = {currentSelected}", gameObject);
    }
    public void Deselect(ISelectable cachedSelected)
    {
        cachedSelected?.Highlight.HighlightOff();
        cachedSelected?.OnDeselected();
    }
    void Select(ISelectable currentSelected)
    {
        currentSelected?.Highlight.HighlightOn(HighlightType);
        currentSelected?.Highlight.HighlightOn(HighlightType);
        currentSelected?.OnSelected();
    }
    void ContextHandler()
    {
        //TODO resolver isso ¯\(°_o)/¯
        /*
        Desceleciona tudo
        verifica se foi clicado em:
            nada:
                nada
            uma peça
                se essa peça é aliada: 
                    muda seleção
                se essa peça é inimiga: 
                    nada
            um quadrado
                um quadrado vazio:
                    nada
                um quadrado com uma peça
                    se essa peça é aliada
                        muda seleção para peça nela
                    se essa peça é inimiga
                        nada
        */
        // DeselectAll();
        // if (currentSelected == null) return;

        // Piece selectedPiece;

        // Type type = currentSelected.GetType();
        // if (type == typeof(BoardPiece))
        // {
        //     Piece piece = (currentSelected as BoardPiece).currentPiece;
        //     if (!piece) return;
        //     else selectedPiece = piece;
        // }
        // else selectedPiece = currentSelected as Piece;

        // // if (GameManager.Instance.IsTurn(selectedPiece.playerData))
        // selectedPiece.OnSelected();
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
}

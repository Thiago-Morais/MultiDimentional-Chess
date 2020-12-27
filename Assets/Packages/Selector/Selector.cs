using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Selector : MonoBehaviour, /* IMediatorInstance<Selector, Selector.IntFlags> */ IMediator<Selector.IntFlags>
{
    #region -------- FIELDS
    public ISelectable currentSelected;
    [SerializeField] Camera m_Camera;
    [SerializeField] LayerMask selectionMask;
    Vector2 m_PointerPosition;
    ISelectable m_CachedSelected;
    // MediatorConcrete<Selector, IntFlags> mediator = new MediatorConcrete<Selector, IntFlags>();
    #endregion //FIELDS

    #region -------- PROPERTIES
    // public MediatorConcrete<Selector, IntFlags> Mediator { get => mediator; set => mediator = value; }
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
        // SignOn(this);
        SignOn();
        if (!m_Camera) m_Camera = Camera.main;
    }

    #region -------- METHODS
    #region -------- MEDIATOR
    public void SignOn() => ContextMediator.SignOn(this);
    public void Notify(IntFlags intFlag) => ContextMediator.Notify(this, intFlag);
    // public void SignOn() => Mediator.SignOn(this);
    // public void Notify(IntFlags intFlag) => Mediator.Notify(this, intFlag);
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
        // if (newSelected == null) return;

        ChangeSelection(newSelected);
    }
    // void ChangeSelection(ISelectable newSelected)
    // {
    //     currentSelected?.OnDeselected();
    //     newSelected?.OnSelected();
    //     currentSelected = newSelected;
    //     print(currentSelected);
    // }
    public void ChangeSelection(ISelectable newSelected)
    {
        m_CachedSelected = currentSelected;
        currentSelected = newSelected;
        m_CachedSelected?.OnDeselected();
        currentSelected?.OnSelected();

        ContextHandler();
        Notify(IntFlags.SelectionChanged);
        print(currentSelected);
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
        DeselectAll();
        if (currentSelected == null) return;

        Piece selectedPiece;

        Type type = currentSelected.GetType();
        if (type == typeof(BoardPiece))
        {
            Piece piece = (currentSelected as BoardPiece).currentPiece;
            if (!piece) return;
            else selectedPiece = piece;
        }
        else selectedPiece = currentSelected as Piece;

        // if (GameManager.Instance.IsTurn(selectedPiece.playerData))
        selectedPiece.OnSelected();
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

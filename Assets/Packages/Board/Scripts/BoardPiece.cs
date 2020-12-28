using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPiece : MonoBehaviour, IPoolable, ISelectable,/*  IMediatorInstance<BoardPiece, BoardPiece.IntFlags>, */IMediator<BoardPiece.IntFlags>, IOnBoard
{
    #region -------- FIELDS
    public SO_BoardSquare so_pieceData;
    [SerializeField] Highlight highlight;
    public Transform pieceTarget;
    [SerializeField] Vector3Int boardCoord;
    public Piece currentPiece;
    // MediatorConcrete<BoardPiece, IntFlags> mediator = new MediatorConcrete<BoardPiece, IntFlags>();
    #endregion //FIELDS
    #region -------- PROPERTIES
    public Vector3Int BoardCoord { get => boardCoord; set => boardCoord = value; }        //TODO setar posição do square no inicio
    public Highlight Highlight { get => highlight; set => highlight = value; }

    // public MediatorConcrete<BoardPiece, IntFlags> Mediator { get => mediator; set => mediator = value; }
    #endregion //PROPERTIES
    void Awake()
    {
        if (!Highlight) Highlight = GetComponentInChildren<Highlight>();
        SignOn();
    }
    public enum IntFlags
    {
        none = 0,
        Selected = 1 << 0,
        Deselected = 1 << 1,
    }
    #region -------- METHODS
    #region -------- MEDIATOR
    public void SignOn() => ContextMediator.SignOn(this);
    public void Notify(IntFlags intFlag) => ContextMediator.Notify(this, intFlag);
    // public void SignOn() => Mediator.SignOn(this);
    // public void Notify(IntFlags intFlag) => Mediator.Notify(this, intFlag);
    #endregion //MEDIATOR
    public void OnSelected()
    {
        Notify(IntFlags.Selected);
        // highlight.LockHighlight();
        // highlightSelected
    }
    public void OnDeselected()
    {
        Notify(IntFlags.Deselected);
        Highlight.HighlightOff();
        // highlight.UnlockHighlight();
    }
    [ContextMenu(nameof(UpdateSize))]
    public void UpdateSize() => so_pieceData.UpdateSize();
    public IPoolable Deactivated()
    {
        gameObject.SetActive(false);
        return this;
    }
    public IPoolable Activated()
    {
        gameObject.SetActive(true);
        return this;
    }
    public IPoolable Instantiate(Transform poolParent) => Instantiate(so_pieceData.prefab, poolParent).GetComponent<BoardPiece>();
    #endregion //METHODS
}

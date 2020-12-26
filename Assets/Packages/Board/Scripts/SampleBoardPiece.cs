using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleBoardPiece : MonoBehaviour, IPoolable, ISelectable, IMediator<SampleBoardPiece, SampleBoardPiece.IntFlags>, IOnBoard
{
    #region -------- FIELDS
    public SO_BoardSquare so_pieceData;
    public Transform pieceTarget;
    public Highlight highlight;
    [SerializeField] Vector3Int boardCoord;

    #endregion //FIELDS
    #region -------- PROPERTIES
    public Vector3Int BoardCoord { get => boardCoord; set => boardCoord = value; }        //TODO setar posição do square no inicio
    public bool Selected { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    #endregion //PROPERTIES
    void Awake()
    {
        if (!highlight) highlight = GetComponentInChildren<Highlight>();
        SignOn(this);
    }
    public enum IntFlags { }

    #region -------- METHODS
    #region -------- MEDIATOR
    public void SignOn(SampleBoardPiece sender)
    {
        ContextMediator.SignOn(this);
    }
    public void Notify(SampleBoardPiece sender, IntFlags intFlag)
    {
    }
    #endregion //MEDIATOR
    public void OnSelected()
    {
        // highlight.LockHighlight();
        // highlightSelected
    }
    public void OnDeselected()
    {
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
    public IPoolable Instantiate(Transform poolParent) => Instantiate(so_pieceData.prefab, poolParent).GetComponent<SampleBoardPiece>();
    #endregion //METHODS
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public class SamplePiece : MonoBehaviour, ISelectable, IMediator<SamplePiece, SamplePiece.IntFlags>, IHighlightable, IOnBoard
{
    #region -------- FIELDS
    public SampleBoardPiece targetSquare;
    public SampleBoardPiece currentSquare;
    public Transform targetTransform;
    public Highlight highlight;
    public IntFlags intFlags;
    [SerializeField] Vector3Int boardCoord;
    public bool isWhite = true;
    public PieceMoveSet moveSet;
    public PieceMoveSet captureSet;
    #endregion //FIELDS
    #region -------- PROPERTIES
    public Vector3Int BoardCoord { get => boardCoord; set => boardCoord = value; }
    public bool Selected
    {
        get => intFlags.HasAny(IntFlags.Selected);
        set
        {
            if (value) intFlags = intFlags.Include(IntFlags.Selected);
            else intFlags = intFlags.Exclude(IntFlags.Selected);
        }
    }
    public Highlight Highlight { get => highlight; set => highlight = value; }
    #endregion //PROPERTIES
    [Flags]
    public enum IntFlags
    {
        none = 0,
        Selected = 1 << 0,
    }
    void Awake()
    {
        InitializeVariables();
        SignOn(this);
    }
    void InitializeVariables()
    {
        if (!highlight) highlight = GetComponent<Highlight>();
    }
    #region -------- METHODS
    #region -------- MEDIATOR
    public void SignOn(SamplePiece sender)
    {
        ContextMediator.SignOn(sender);
    }
    public void Notify(SamplePiece sender, IntFlags intFlag)
    {
        ContextMediator.Notify(sender, intFlag);
    }
    #endregion //MEDIATOR
    [ContextMenu(nameof(MoveUsingTransform))]
    public void MoveUsingTransform() => MoveTo(targetTransform);
    public void MoveTo(Transform target)
    {
        transform.position = target.position;
    }
    [ContextMenu(nameof(MoveUsingSO))]
    public void MoveUsingSO() => MoveTo(targetSquare);
    public void MoveTo(SampleBoardPiece target)
    {
        transform.position = target.pieceTarget.position;
        currentSquare = target;
        BoardCoord = target.BoardCoord;
    }
    public void OnSelected()
    {
        // highlight.LockHighlight();
        Notify(this, IntFlags.Selected);
    }
    public void OnDeselected()
    {
        // highlight.UnlockHighlight();
    }
    public bool IsMoveAvailable(SampleBoardPiece square, PieceMoveSet moveSet)
    {
        Vector3Int dif = square.BoardCoord - BoardCoord;

        return moveSet.IsMovimentAvailable(dif, isWhite);
    }
    #endregion //METHODS
}

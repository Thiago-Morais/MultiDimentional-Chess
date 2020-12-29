﻿using System;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

[RequireComponent(typeof(Highlight))]
public class Piece : MonoBehaviour, ISelectable, IMediator<Piece.IntFlags>, IHighlightable, IOnBoard
{
    #region -------- FIELDS
    public BoardPiece currentSquare;
    public BoardPiece cachedSquare;
    public Transform targetTransform;
    public Highlight highlight;
    public IntFlags intFlags;
    [SerializeField] Vector3Int boardCoord;
    public PlayerData playerData;
    public PieceMoveSet moveSet;
    public PieceMoveSet captureSet;
    #endregion //FIELDS
    #region -------- PROPERTIES
    public Vector3Int BoardCoord { get => boardCoord; set => boardCoord = value; }
    public Highlight Highlight { get => highlight; set => highlight = value; }
    #endregion //PROPERTIES
    [Flags]
    public enum IntFlags
    {
        none = 0,
        Selected = 1 << 0,
        Deselected = 1 << 1,
        UpdateTarget = 1 << 2,
        MoveToCoord = 1 << 3,
        ShowPossibleMoves = 1 << 4,
    }
    public void Awake()
    {
        InitializeVariables();
        SignOn();
        playerData.ApplyPlayerData(this);
    }
    public void Start() => MoveToCoord();
    public void InitializeVariables()
    {
        highlight = highlight.Initialized(this);
        if (!playerData) playerData = ScriptableObject.CreateInstance<PlayerData>();
        if (!moveSet) moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
        if (!captureSet) captureSet = ScriptableObject.CreateInstance<PieceMoveSet>();
    }

    #region -------- METHODS
    #region -------- MEDIATOR
    public void SignOn()
        => ContextMediator.SignOn(this);
    public void Notify(IntFlags intFlag)
        => ContextMediator.Notify(this, intFlag);
    #endregion //MEDIATOR

    public void MoveToCoord(Vector3Int boardCoord)
    {
        BoardCoord = boardCoord;
        MoveToCoord();
    }
    [ContextMenu(nameof(MoveToCoord))]
    public void MoveToCoord()
    {
        Notify(IntFlags.MoveToCoord);
    }
    public void MoveTo(BoardPiece target)
    {
        if (!target) return;

        cachedSquare = currentSquare;

        target.currentPiece = this;
        transform.position = target.pieceTarget.position;
        BoardCoord = target.BoardCoord;

        currentSquare = target;
    }
    public void OnSelected()
    {
        HighlightPossibleMoves();
        Notify(IntFlags.Selected);
    }
    public void OnDeselected()
    {
    }
    public void HighlightPossibleMoves()
    {
        Notify(IntFlags.ShowPossibleMoves);
    }
    public bool IsAnyMovimentAvailable(BoardPiece square) => IsMovimentAvailable(square, moveSet) || IsMovimentAvailable(square, captureSet);
    public bool IsMoveAvailable(BoardPiece square) => IsMovimentAvailable(square, moveSet);
    public bool IsCaptureAvailable(BoardPiece square) => IsMovimentAvailable(square, captureSet);
    public bool IsMovimentAvailable(BoardPiece square) => IsMovimentAvailable(square, moveSet);
    public bool IsMovimentAvailable(BoardPiece square, PieceMoveSet moveSet)
    {
        Vector3Int dif = square.BoardCoord - BoardCoord;

        return moveSet.IsMovimentAvailable(dif, playerData.isWhite);
    }
    #endregion //METHODS
}

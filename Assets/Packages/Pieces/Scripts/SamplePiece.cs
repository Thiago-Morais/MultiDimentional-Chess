using System;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public class SamplePiece : MonoBehaviour, ISelectable, IMediator<SamplePiece, SamplePiece.IntFlags>, IHighlightable, IOnBoard
{
    #region -------- FIELDS
    public BoardPiece targetSquare;
    public BoardPiece currentSquare;
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
        Deselected = 1 << 1,
        UpdateTarget = 1 << 2,
    }
    void Awake()
    {
        InitializeVariables();
        SignOn(this);
    }
    void Start() => MoveToCoord();
    void InitializeVariables()
    {
        if (!highlight) highlight = GetComponent<Highlight>();
    }
    #region -------- METHODS
    #region -------- MEDIATOR
    public void SignOn(SamplePiece sender)
        => ContextMediator.SignOn(sender);
    public void Notify(IntFlags intFlag)
        => ContextMediator.Notify(this, intFlag);
    #endregion //MEDIATOR

    [ContextMenu(nameof(MoveToCoord))]
    public void MoveToCoord()
    {
        UpdateSquareTarget();
        MoveUsingSquare();
    }
    [ContextMenu(nameof(UpdateSquareTarget))]
    public void UpdateSquareTarget()
    {
        Notify(IntFlags.UpdateTarget);
    }
    [ContextMenu(nameof(MoveUsingSquare))]
    public void MoveUsingSquare() => MoveTo(targetSquare);
    public void MoveTo(BoardPiece target)
    {
        if (!target) return;
        transform.position = target.pieceTarget.position;
        currentSquare = target;
        BoardCoord = target.BoardCoord;
    }
    public void OnSelected()
    {
        Notify(IntFlags.Selected);
    }
    public void OnDeselected()
    {
        Notify(IntFlags.Deselected);
    }
    public bool IsAnyMovimentAvailable(BoardPiece square) => IsMovimentAvailable(square, moveSet) || IsMovimentAvailable(square, captureSet);
    public bool IsMoveAvailable(BoardPiece square) => IsMovimentAvailable(square, moveSet);
    public bool IsCaptureAvailable(BoardPiece square) => IsMovimentAvailable(square, captureSet);
    public bool IsMovimentAvailable(BoardPiece square, PieceMoveSet moveSet)
    {
        Vector3Int dif = square.BoardCoord - BoardCoord;

        return moveSet.IsMovimentAvailable(dif, isWhite);
    }
    #endregion //METHODS
}

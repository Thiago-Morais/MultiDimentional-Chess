using System;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public partial class Piece : MonoBehaviour, ISelectable/* , IMediatorInstance<Piece, Piece.IntFlags> */, IMediator<Piece.IntFlags>, IHighlightable, IOnBoard
{
    #region -------- FIELDS
    // public BoardPiece targetSquare;
    public BoardPiece currentSquare;
    public BoardPiece cachedSquare;
    public Transform targetTransform;
    public Highlight highlight = new Highlight();
    public IntFlags intFlags;
    [SerializeField] Vector3Int boardCoord;
    public PlayerData playerData;
    // public bool isWhite = true;
    public PieceMoveSet moveSet;
    public PieceMoveSet captureSet;
    // MediatorConcrete<Piece, Piece.IntFlags> mediator = new MediatorConcrete<Piece, Piece.IntFlags>();
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
    // public MediatorConcrete<Piece, IntFlags> Mediator { get => mediator; set => mediator = value; }
    #endregion //PROPERTIES
    [Flags]
    public enum IntFlags
    {
        none = 0,
        Selected = 1 << 0,
        Deselected = 1 << 1,
        UpdateTarget = 1 << 2,
        MoveToCoord = 1 << 3,
    }
    void Awake()
    {
        InitializeVariables();
        SignOn();
        playerData.ApplyPlayerData(this);
    }
    void Start() => MoveToCoord();
    void InitializeVariables()
    {
        if (!highlight) highlight = GetComponent<Highlight>();
    }

    #region -------- METHODS
    #region -------- MEDIATOR
    public void SignOn()
        => ContextMediator.SignOn(this);
    public void Notify(IntFlags intFlag)
        => ContextMediator.Notify(this, intFlag);
    // public void SignOn()
    //     => Mediator.SignOn(this);
    // public void Notify(IntFlags intFlag)
    //     => Mediator.Notify(this, intFlag);
    #endregion //MEDIATOR

    [ContextMenu(nameof(MoveToCoord))]
    public void MoveToCoord()
    {
        Notify(IntFlags.MoveToCoord);
        // UpdateSquareTarget();
        // MoveUsingTarget();
    }
    // [ContextMenu(nameof(UpdateSquareTarget))]
    // public void UpdateSquareTarget()
    // {
    //     Notify(IntFlags.UpdateTarget);
    //     // Mediator.Notify(this, IntFlags.UpdateTarget);
    // }
    // [ContextMenu(nameof(MoveUsingTarget))]
    // public void MoveUsingTarget() => MoveTo(targetSquare);
    public void MoveTo(BoardPiece target)
    {
        if (!target) return;

        cachedSquare = currentSquare;

        target.currentPiece = this;
        transform.position = target.pieceTarget.position;
        // targetSquare = target;
        BoardCoord = target.BoardCoord;

        currentSquare = target;
    }
    public void OnSelected()
    {
        Notify(IntFlags.Selected);
        // Mediator.Notify(this, IntFlags.Selected);
    }
    public void OnDeselected()
    {
    }
    public bool IsAnyMovimentAvailable(BoardPiece square) => IsMovimentAvailable(square, moveSet) || IsMovimentAvailable(square, captureSet);
    public bool IsMoveAvailable(BoardPiece square) => IsMovimentAvailable(square, moveSet);
    public bool IsCaptureAvailable(BoardPiece square) => IsMovimentAvailable(square, captureSet);
    public bool IsMovimentAvailable(BoardPiece square, PieceMoveSet moveSet)
    {
        Vector3Int dif = square.BoardCoord - BoardCoord;

        // return moveSet.IsMovimentAvailable(dif, isWhite);
        return moveSet.IsMovimentAvailable(dif, playerData.isWhite);
    }
    #endregion //METHODS
}

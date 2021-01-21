using System;
using ExtensionMethods;
using UnityEngine;

public class Piece : MonoBehaviour, ISelectable, IMediator<Piece.IntFlags>, IHighlightable, IOnBoard
{
    #region -------- FIELDS
    public BoardPiece currentSquare;
    public BoardPiece cachedSquare;
    public Highlight highlight;
    [SerializeField] Vector3Int boardCoord;
    public PlayerData playerData;
    public PieceMoveSet moveSet;
    public PieceMoveSet captureSet;
    #endregion //FIELDS

    #region -------- PROPERTIES
    public Vector3Int BoardCoord { get => boardCoord; set => boardCoord = value; }
    public Highlight Highlight { get => highlight; set => highlight = value; }
    #endregion //PROPERTIES

    #region -------- OUTSIDE CALL
    public void Awake()
    {
        InitializeVariables();
        SignOn();
        playerData.ApplyPlayerData(this);
    }
    public void Start() => MoveToCoord();
    #endregion //OUTSIDE CALL

    #region -------- METHODS
    public void InitializeVariables()       //TODO test it
    {
        if (!highlight) highlight = highlight.Initialized(this);
        if (!highlight) highlight = gameObject.AddComponent<Highlight>().Initialized(transform) as Highlight;
        if (!playerData) playerData = ScriptableObject.CreateInstance<PlayerData>();
        if (!moveSet) moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
        if (!captureSet) captureSet = ScriptableObject.CreateInstance<PieceMoveSet>();
    }
    #region -------- MEDIATOR
    public void SignOn()
        => ContextMediator.SignOn(this);
    public void Notify(IntFlags intFlag)
        => ContextMediator.Notify(this, intFlag);
    #endregion //MEDIATOR

    public void MoveToCoord(Vector3Int boardCoord)      //TODO test it
    {
        BoardCoord = boardCoord;
        MoveToCoord();
    }
    [ContextMenu(nameof(MoveToCoord))]
    public void MoveToCoord()       //TODO test it
    {
        Notify(IntFlags.MoveToCoord);
    }
    public void MoveTo(BoardPiece target)       //TODO test it
    {
        if (!target) return;

        cachedSquare = currentSquare;

        target.currentPiece = this;
        transform.position = target.pieceTarget.position;
        BoardCoord = target.BoardCoord;

        currentSquare = target;
    }
    public void OnSelected()        //TODO test it
    {
        HighlightPossibleMoves();
    }
    public void OnDeselected()      //TODO test it
    {
        highlight.HighlightUndo();
        UnHighlightPossibleMoves();
    }
    public void UnHighlightPossibleMoves() => Notify(IntFlags.HidePossibleMoves);       //TODO test it
    public void HighlightPossibleMoves() => Notify(IntFlags.ShowPossibleMoves);     //TODO test it
    public bool IsAnyMovimentAvailable(BoardPiece square) => IsMovimentAvailable(square) || IsCaptureAvailable(square);     //TODO test it
    public bool IsCaptureAvailable(BoardPiece square) => IsMovimentAvailableWith(square, captureSet);       //TODO test it
    public bool IsMovimentAvailable(BoardPiece square) => IsMovimentAvailableWith(square, moveSet);     //TODO test it
    bool IsMovimentAvailableWith(BoardPiece square, PieceMoveSet moveSet)       //TODO test it
    {
        return moveSet.IsMovimentAvailable(this, square);
    }
    #endregion //METHODS

    #region -------- ENUM
    [Flags]
    public enum IntFlags
    {
        none = 0,
        MoveToCoord = 1 << 1,
        ShowPossibleMoves = 1 << 2,
        HidePossibleMoves = 1 << 3,
    }
    #endregion //ENUM
}

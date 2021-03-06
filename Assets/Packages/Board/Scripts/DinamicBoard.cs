﻿using System;
using ExtensionMethods;
using UnityEngine;

public partial class DinamicBoard : MonoBehaviour, IMediator<DinamicBoard.IntFlags>, IInitializable
{
    #region FIELDS
    public BoardPiece[,,] board;
    [Header("Board Position")]
    public Vector3Int size = new Vector3Int(8, 1, 8);
    public Vector3 padding = Vector3.up * 2;
    public Transform center;
    [Header("Board pieces")]
    [SerializeField] Pool whitePool;
    [SerializeField] Pool blackPool;
    Vector3 cachePadding = new Vector3();
    Vector3 cacheSize = new Vector3();
    #endregion //FIELDS

    #region -------- PROPERTIES
    public Pool BlackPool { get => blackPool; private set => blackPool = value; }
    public Pool WhitePool { get => whitePool; private set => whitePool = value; }
    #endregion //PROPERTIES

    #region -------- OUTSIDE CALL
    public void Awake()
    {
        SignOn();
        InitializeVariables();
        TryUpdateBoard();
    }
    void Update() => TryUpdateBoard();
    #endregion //OUTSIDE CALL

    #region -------- METHODS
    public void InitializeVariables()
    {
        if (size == null) size = new Vector3Int();
        if (padding == null) padding = new Vector3();
        if (!center)
        {
            center = new GameObject(nameof(center)).transform;
            center.SetParent(transform);
        }
        if (board == null) board = new BoardPiece[0, 0, 0];
        if (!WhitePool)
        {
            WhitePool = new GameObject(nameof(WhitePool)).AddComponent<Pool>().InitializedAs<BoardPiece>();
            WhitePool.transform.SetParent(transform);
        }
        if (!BlackPool)
        {
            BlackPool = new GameObject(nameof(BlackPool)).AddComponent<Pool>().InitializedAs<BoardPiece>();
            BlackPool.transform.SetParent(transform);
        }
    }
    #region ------------ MEDIATOR
    public void SignOn()
        => ContextMediator.SignOn(this);
    public void Notify(IntFlags intFlag)
        => ContextMediator.Notify(this, intFlag);
    #endregion //MEDIATOR
    [ContextMenu(nameof(TryUpdateBoard))] public void TryUpdateBoard() => TryUpdateBoard(size, padding);        //TODO test it
    public void TryUpdateBoard(Vector3Int size, Vector3 padding)        //TODO test it
    {
        if (DidSizeChanged(size))
        {
            ResetBoardSize(size);
            UpdateBoardPieces();
        }
        UpdatePadding(padding);
        UpdateCenter();
    }
    [ContextMenu(nameof(ForceUpdateBoard))]
    public void ForceUpdateBoard() => ForceUpdateBoard(size, padding);      //TODO test it
    public void ForceUpdateBoard(Vector3Int size, Vector3 padding)      //TODO test it
    {
        ResetBoardSize(size);
        UpdatePadding(padding);
        UpdateCenter();
        UpdateBoardPieces();
    }
    [ContextMenu(nameof(UpdateCenter))]
    public void UpdateCenter()      //TODO test it
    {
        Vector3 whiteSquareSize = GetSquareSize(0);
        Vector3 blackSquareSize = GetSquareSize(0);

        int x = board.GetLength(0);
        int y = board.GetLength(1);
        int z = board.GetLength(2);
        Vector3 boardCount = new Vector3(x, y, z) / 2;

        Vector3 whitePadded = whiteSquareSize + padding;
        Vector3 blackPadded = blackSquareSize + padding;

        Vector3 whiteSizeMultiplied = whitePadded.Scaled(boardCount);
        Vector3 blackSizeMultiplied = blackPadded.Scaled(boardCount);

        if (center) center.localPosition = (whiteSizeMultiplied + blackSizeMultiplied - whiteSquareSize - padding) / 2;
    }
    [ContextMenu(nameof(PrintBoard))] void PrintBoard() { foreach (BoardPiece square in board) Debug.Log(square, square); }

    #region ------------ CHANGE VERIFICATION
    bool DidValuesChange(Vector3Int size, Vector3 padding) => DidSizeChanged(size) || DidPaddingChanged(padding);       //TODO test it
    bool DidSizeChanged(Vector3Int size) => size != cacheSize;      //TODO test it
    bool DidPaddingChanged(Vector3 padding) => padding != cachePadding;     //TODO test it
    #endregion //CHANGE VERIFICATION

    #region ------------ BOARD GENERATION
    public void ResetBoardSize(Vector3Int size)         //TODO test it
    {
        if (board == null) board = InstantiateABoard(size);
        BoardPiece[,,] newBoard = GenerateBoardFromOld(size);
        DestroyBoard(board);
        board = newBoard;
        cacheSize = size;
    }
    public static BoardPiece[,,] InstantiateABoard(Vector3Int size)
        => new BoardPiece[(int)Mathf.Clamp(size.x, 0, Mathf.Infinity), (int)Mathf.Clamp(size.y, 0, Mathf.Infinity), (int)Mathf.Clamp(size.z, 0, Mathf.Infinity)];
    BoardPiece[,,] GenerateBoardFromOld(Vector3Int size) => ResizeUsing(InstantiateABoard(size), board);     //TODO test it
    BoardPiece[,,] ResizeUsing(BoardPiece[,,] newBoard, BoardPiece[,,] board)       //TODO test it
    {
        return newBoard.ForEachDoAction((i, j, k) =>
        {
            if (board.OutOfBounds(i, j, k) || board[i, j, k] == null)
                newBoard[i, j, k] = GenerateBoardPieceUsing(i + j + k);
            else
                newBoard[i, j, k] = board.RemoveAt(i, j, k);
        });
    }
    public BoardPiece GenerateBoardPieceUsing(int index)
    {
        Pool genericPool = GetPool(index);
        BoardPiece boardPiece = genericPool.GetFromPoolGrouped<BoardPiece>();
        return boardPiece;
    }
    void DestroyBoard(BoardPiece[,,] board)     //TODO test it
    {
        Action<int, int, int> PushToPool = (i, j, k) =>
        {
            Pool squarePool = GetPool(i + j + k);
            squarePool.PushToPool(board[i, j, k]);
        };
        board.ForEachDoAction(PushToPool);
    }
    void UpdatePadding(Vector3 padding)     //TODO test it
    {
        Vector3 index = new Vector3();
        for (int i = 0; i < board.GetLength(0); i++)
            for (int j = 0; j < board.GetLength(1); j++)
                for (int k = 0; k < board.GetLength(2); k++)
                {
                    index.Set(i, j, k);
                    Vector3 squareSize = GetSquareSize(i + j + k);
                    Vector3 squarePosition = squareSize.Scaled(index);
                    Vector3 squarePadding = padding.Scaled(index);

                    board[i, j, k].transform.position = squarePosition + squarePadding;
                }
        cachePadding = padding;
    }
    Vector3 GetSquareSize(int index)                                //TODO test it
    {
        BoardPiece boardPiece = ((BoardPiece)this.GetPool(index).sample);
        return boardPiece.GetSize();
    }
    [ContextMenu(nameof(UpdateBoardPieces))]
    public void UpdateBoardPieces()
    {
        UpdateBoardCoord();
        UpdateBoardPiecesBoardReference();
    }
    public void UpdateBoardPiecesBoardReference() => board.ForEachDoAction(UpdateBoardPiecesBoardAt);
    [ContextMenu(nameof(UpdateBoardPiecesBoardAt))]
    public void UpdateBoardPiecesBoardAt(int i, int j, int k) => board[i, j, k].board = this;
    [ContextMenu(nameof(UpdateBoardCoord))]
    public void UpdateBoardCoord() => board.ForEachDoAction(UpdateBoardPieceCoordAt);         //TODO test it
    public void UpdateBoardPieceCoordAt(int i, int j, int k) => board[i, j, k].BoardCoord = new Vector3Int(i, j, k);          //TODO test it
    #endregion //BOARD GENERATION
    public BoardPiece GetSquareAt(Vector3Int boardCoord) => GetSquareAt(boardCoord.x, boardCoord.y, boardCoord.z);
    public BoardPiece GetSquareAt(int boardCoordX, int boardCoordY, int boardCoordZ)
    {
        if (board.OutOfBounds(boardCoordX, boardCoordY, boardCoordZ)) return default(BoardPiece);
        return board[boardCoordX, boardCoordY, boardCoordZ];
    }
    public bool? IsMovementAvailable(Piece piece, Vector3Int boardCoord)        //TODO test it
    {
        BoardPiece boardPiece = GetSquareAt(boardCoord);
        return piece.IsMovementAvailable(boardPiece);
    }
    public Pool GetPool(int index) => index.IsEven() ? WhitePool : BlackPool;
    public BoardPiece[,,] GenerateBoard(Vector3Int boardSize)
    {
        BoardPiece[,,] boardPieces = InstantiateABoard(boardSize);
        boardPieces.ForEachDoAction((i, j, k) => boardPieces[i, j, k] = GetPool(i + j + k).GetFromPoolGrouped<BoardPiece>());
        return boardPieces;
    }
    public bool HasPieceBetween(Vector3Int coordA, Vector3Int coordB)
    {
        /*
        verificar o rank da direção pra saber qual verificação fazer
        se não cair na verificação, não tem peça na frente
        verificação 
            pega o valor maximo da direção   
            verifica cada peça até o valor maximo 
                se qualquer uma tiver um peça
                    true
            se chegar no maximo
                false
        */
        Vector3Int direction = coordB - coordA;
        Binding binding = Binding.IsBindedIgnoringZero(direction);
        if (binding.IsBinded)
        {
            for (int i = 0; i < Mathf.Abs(binding.binded[0]); i++)
            {
                Vector3Int nextSquareForDirection = coordA + binding.signVector * (i + 1);
                if (GetSquareAt(nextSquareForDirection)?.currentPiece)
                    return true;
            }
        }
        return false;
    }
    public IInitializable Initialized(Transform parent = null)
    {
        InitializeVariables();
        transform.parent = parent;
        return this;
    }
    #endregion //METHODS

    #region -------- ENUM
    [Flags] public enum IntFlags { }
    #endregion //ENUM
}

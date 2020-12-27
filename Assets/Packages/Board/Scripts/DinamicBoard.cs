using System;
using System.Collections;
using ExtensionMethods;
using UnityEngine;

public partial class DinamicBoard : MonoBehaviour,/* IMediatorInstance<DinamicBoard, DinamicBoard.IntFlags> */IMediator<DinamicBoard.IntFlags>
{
    #region FIELDS
    public Vector3Int size;
    public Vector3 padding;
    public Transform center;
    public BoardPiece[,,] board;
    public Pool<BoardPiece> whitePool;
    public Pool<BoardPiece> blackPool;
    public Vector3 offset;
    Vector3 cachePadding;
    Vector3 cacheSize;
    Vector3 cacheCenter;
    // MediatorConcrete<DinamicBoard, IntFlags> mediator = new MediatorConcrete<DinamicBoard, IntFlags>();
    #endregion //FIELDS

    #region -------- PROPERTIES
    // public MediatorConcrete<DinamicBoard, IntFlags> Mediator { get => mediator; set => mediator = value; }
    #endregion //PROPERTIES
    [Flags]
    public enum IntFlags
    {
    }
    void Awake()
    {
        SignOn();
        TryUpdateBoard();
    }
    void Update() => TryUpdateBoard();

    #region -------- METHODS
    #region ------------ MEDIATOR
    public void SignOn()
        => ContextMediator.SignOn(this);
    public void Notify(IntFlags intFlag)
        => ContextMediator.Notify(this, intFlag);
    // public void SignOn()
    //     => Mediator.SignOn(this);
    // public void Notify(IntFlags intFlag)
    //     => Mediator.Notify(this, intFlag);
    #endregion //MEDIATOR
    [ContextMenu(nameof(SetPools))]
    void SetPools()
    {
        whitePool.SetSample();
        blackPool.SetSample();
    }
    [ContextMenu(nameof(TryUpdateBoard))] void TryUpdateBoard() => TryUpdateBoard(size, padding);
    public void TryUpdateBoard(Vector3Int size, Vector3 padding)
    {
        // if (!DidValuesChange(size, padding)) return;

        if (DidSizeChanged(size))
        {
            ResetBoardSize(size);
            SetSquareCoordenates();
        }
        UpdatePadding(padding);
        UpdateCenter();
    }
    [ContextMenu(nameof(ForceUpdateBoard))]
    public void ForceUpdateBoard() => ForceUpdateBoard(size, padding);
    public void ForceUpdateBoard(Vector3Int size, Vector3 padding)
    {
        ResetBoardSize(size);
        UpdatePadding(padding);
        UpdateCenter();
        SetSquareCoordenates();
    }
    [ContextMenu(nameof(UpdateCenter))]
    public void UpdateCenter()
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

        center.localPosition = (whiteSizeMultiplied + blackSizeMultiplied - whiteSquareSize - padding + offset) / 2;
    }
    [ContextMenu(nameof(PrintBoard))] void PrintBoard() { foreach (BoardPiece square in board) Debug.Log(square, square); }

    #region ------------ CHANGE VERIFICATION
    bool DidValuesChange(Vector3Int size, Vector3 padding) => DidSizeChanged(size) || DidPaddingChanged(padding);
    bool DidSizeChanged(Vector3Int size) => size != cacheSize;
    bool DidPaddingChanged(Vector3 padding) => padding != cachePadding;
    #endregion //CHANGE VERIFICATION

    #region ------------ BOARD GENERATION
    void ResetBoardSize(Vector3Int size)
    {
        if (board == null) InitializeBoard(size);
        BoardPiece[,,] newBoard = GenerateBoardFromOld(size);
        DestroyBoard(board);
        board = newBoard;
        cacheSize = size;
    }
    BoardPiece[,,] InitializeBoard(Vector3Int size) => board = InstantiateABoard(size);
    static BoardPiece[,,] InstantiateABoard(Vector3Int size) => new BoardPiece[size.x, size.y, size.z];

    BoardPiece[,,] GenerateBoardFromOld(Vector3Int size) => ResizeUsing(InstantiateABoard(size), board);
    BoardPiece[,,] ResizeUsing(BoardPiece[,,] newBoard, BoardPiece[,,] board)
    {
        return newBoard.ForEachDoAction((i, j, k) =>
        {
            if (board.OutOfBounds(i, j, k) || board[i, j, k] == null)
                newBoard[i, j, k] = GenerateSquareUsing(i + j + k);
            else
                newBoard[i, j, k] = board.RemoveAt(i, j, k);
        });
    }
    BoardPiece GenerateSquareUsing(int index)
    {
        Pool<BoardPiece> genericPool = GetPool(index);
        return genericPool.GetFromPool(genericPool.sample);
    }

    void DestroyBoard(BoardPiece[,,] board)
    {
        Action<int, int, int> PushToPool = (i, j, k) =>
        {
            Pool<BoardPiece> squarePool = GetPool(i + j + k);
            squarePool.PushToPool(board[i, j, k]);
        };
        board.ForEachDoAction(PushToPool);
    }
    void UpdatePadding(Vector3 padding)
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
    Vector3 GetSquareSize(int index) => this.GetPool(index).sample.so_pieceData.pieceBounds.size;
    Pool<BoardPiece> GetPool(int index) => IsPair(index) ? whitePool : blackPool;
    [ContextMenu(nameof(SetSquareCoordenates))]
    void SetSquareCoordenates() => board.ForEachDoAction(SetSquareCoordenates);
    void SetSquareCoordenates(int i, int j, int k) => board[i, j, k].BoardCoord = new Vector3Int(i, j, k);
    #endregion //BOARD GENERATION

    public BoardPiece GetSquareAt(Vector3Int boardCoord)
    {
        if (board.OutOfBounds(boardCoord.x, boardCoord.y, boardCoord.z)) return default(BoardPiece);
        return board[boardCoord.x, boardCoord.y, boardCoord.z];
    }
    static bool IsPair(int value) => value % 2 == 0;
    #endregion //METHODS
}

using System;
using System.Collections;
using ExtensionMethods;
using UnityEngine;

public partial class DinamicBoard : MonoBehaviour, IMediator<DinamicBoard.IntFlags>
{
    #region FIELDS
    public Vector3Int size = new Vector3Int(8, 1, 8);
    public Vector3 padding = Vector3.up * 2;
    public Transform center;
    public BoardPiece[,,] board;
    [SerializeField] Pool<BoardPiece> whitePool = new Pool<BoardPiece>();
    [SerializeField] Pool<BoardPiece> blackPool = new Pool<BoardPiece>();
    public Vector3 offset;
    Vector3 cachePadding = new Vector3();
    Vector3 cacheSize = new Vector3();
    Vector3 cacheCenter = new Vector3();
    #endregion //FIELDS

    #region -------- PROPERTIES
    public Pool<BoardPiece> BlackPool { get => blackPool; private set => blackPool = value; }
    public Pool<BoardPiece> WhitePool { get => whitePool; private set => whitePool = value; }
    #endregion //PROPERTIES

    [Flags]
    public enum IntFlags
    {
    }
    public void Awake()
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
    #endregion //MEDIATOR
    [ContextMenu(nameof(TryUpdateBoard))] public void TryUpdateBoard() => TryUpdateBoard(size, padding);        //TODO test it
    public void TryUpdateBoard(Vector3Int size, Vector3 padding)        //TODO test it
    {
        // if (!DidValuesChange(size, padding)) return;

        if (DidSizeChanged(size))
        {
            ResetBoardSize(size);
            UpdateBoardCoordAt();
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
        UpdateBoardCoordAt();
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

        if (center) center.localPosition = (whiteSizeMultiplied + blackSizeMultiplied - whiteSquareSize - padding + offset) / 2;
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
        BoardPiece[,,] newBoard = SetBoardFromOld(size);
        DestroyBoard(board);
        board = newBoard;
        cacheSize = size;
    }
    public static BoardPiece[,,] InstantiateABoard(Vector3Int size)
        => new BoardPiece[(int)Mathf.Clamp(size.x, 0, Mathf.Infinity), (int)Mathf.Clamp(size.y, 0, Mathf.Infinity), (int)Mathf.Clamp(size.z, 0, Mathf.Infinity)];
    BoardPiece[,,] SetBoardFromOld(Vector3Int size) => ResizeUsing(InstantiateABoard(size), board);     //TODO test it
    BoardPiece[,,] ResizeUsing(BoardPiece[,,] newBoard, BoardPiece[,,] board)       //TODO test it
    {
        return newBoard.ForEachDoAction((i, j, k) =>
        {
            if (board.OutOfBounds(i, j, k) || board[i, j, k] == null)
                newBoard[i, j, k] = GenerateSquareUsing(i + j + k);
            else
                newBoard[i, j, k] = board.RemoveAt(i, j, k);
        });
    }
    public BoardPiece GenerateSquareUsing(int index)
    {
        Pool<BoardPiece> genericPool = GetPool(index);
        BoardPiece boardPiece = genericPool.GetFromPoolGrouped();
        return boardPiece;
    }
    void DestroyBoard(BoardPiece[,,] board)     //TODO test it
    {
        Action<int, int, int> PushToPool = (i, j, k) =>
        {
            Pool<BoardPiece> squarePool = GetPool(i + j + k);
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
    Vector3 GetSquareSize(int index) => this.GetPool(index).sample.so_pieceData.pieceBounds.size;       //TODO test it
    [ContextMenu(nameof(UpdateBoardCoordAt))]
    void UpdateBoardCoordAt() => board.ForEachDoAction(UpdateBoardCoordAt);         //TODO test it
    public void UpdateBoardCoordAt(int i, int j, int k) => board[i, j, k].BoardCoord = new Vector3Int(i, j, k);          //TODO test it
    #endregion //BOARD GENERATION
    public BoardPiece GetSquareAt(Vector3Int boardCoord)
    {
        if (board.OutOfBounds(boardCoord.x, boardCoord.y, boardCoord.z)) return default(BoardPiece);
        return board[boardCoord.x, boardCoord.y, boardCoord.z];
    }
    public bool? IsMovimentAvailable(Piece piece, Vector3Int boardCoord)        //TODO test it
    {
        BoardPiece boardPiece = GetSquareAt(boardCoord);
        return piece.IsMovimentAvailable(boardPiece);
    }
    public Pool<BoardPiece> GetPool(int index) => index.IsPair() ? WhitePool : BlackPool;
    public BoardPiece[,,] GenerateBoard(Vector3Int boardSize)
    {
        BoardPiece[,,] boardPieces = InstantiateABoard(boardSize);
        boardPieces.ForEachDoAction((i, j, k) => boardPieces[i, j, k] = GetPool(i + j + k).GetFromPoolGrouped());
        return boardPieces;
    }
    #endregion //METHODS
}

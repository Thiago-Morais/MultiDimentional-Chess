using System;
using System.Collections;
using UnityEngine;

public partial class CreateBoard : MonoBehaviour
{
    #region FIELDS
    public Vector3Int size;
    public Vector3 boardCenter;
    public Vector3 padding;
    public SampleBoardPiece[,,] _board;
    public Pool<SampleBoardPiece> whitePool;
    public Pool<SampleBoardPiece> blackPool;
    Vector3 cachePadding;
    Vector3 cacheSize;
    #endregion //FIELDS

    void Start() => TryUpdateBoard();
    void Update() => TryUpdateBoard();

    #region METHODS
    [ContextMenu(nameof(SetPools))]
    void SetPools()
    {
        whitePool.SetSample();
        blackPool.SetSample();
    }
    [ContextMenu(nameof(TryUpdateBoard))] void TryUpdateBoard() => TryUpdateBoard(size, padding);
    public void TryUpdateBoard(Vector3Int size, Vector3 padding)
    {
        if (!DidValuesChange(size, padding)) return;

        if (IsSizeDiferent(size)) ResetBoardSize(size);
        UpdatePadding(padding);

        SetSquareRanks();
    }
    [ContextMenu(nameof(ForceUpdateBoard))]
    public void ForceUpdateBoard() => ForceUpdateBoard(size, padding);
    public void ForceUpdateBoard(Vector3Int size, Vector3 padding)
    {
        ResetBoardSize(size);
        UpdatePadding(padding);
        SetSquareRanks();
    }
    [ContextMenu(nameof(PrintBoard))] void PrintBoard() { foreach (SampleBoardPiece square in _board) Debug.Log(square, square); }

    #region CHANGE VERIFICATION
    bool DidValuesChange(Vector3Int size, Vector3 padding) => IsSizeDiferent(size) || IsPaddingDiferent(padding);
    bool IsSizeDiferent(Vector3Int size) => size != cacheSize;
    bool IsPaddingDiferent(Vector3 padding) => padding != cachePadding;
    #endregion //CHANGE VERIFICATION

    void ResetBoardSize(Vector3Int size)
    {
        if (_board == null) InitializeBoard(size);
        SampleBoardPiece[,,] newBoard = GenerateBoardFromOld(size);
        DestroyBoard();
        _board = newBoard;
        cacheSize = size;
    }
    SampleBoardPiece[,,] InitializeBoard(Vector3Int size) => _board = InstantiateABoard(size);
    static SampleBoardPiece[,,] InstantiateABoard(Vector3Int size) => new SampleBoardPiece[size.x, size.y, size.z];

    #region -------- BOARD GENERATOR
    SampleBoardPiece[,,] GenerateBoardFromOld(Vector3Int size)
    {
        SampleBoardPiece[,,] newBoard = InstantiateABoard(size);
        for (int i = 0; i < newBoard.GetLength(0); i++)
            for (int j = 0; j < newBoard.GetLength(1); j++)
                for (int k = 0; k < newBoard.GetLength(2); k++)
                {
                    if (i > _board.GetUpperBound(0) || j > _board.GetUpperBound(1) || k > _board.GetUpperBound(2) || _board[i, j, k] == null)
                        newBoard[i, j, k] = GenerateSquareUsing(i + j + k);
                    else
                        newBoard[i, j, k] = ExtractSquareUsing(i, j, k);
                }

        return newBoard;
    }
    SampleBoardPiece GenerateSquareUsing(int index)
    {
        Pool<SampleBoardPiece> genericPool = GetPoolUsing(index);
        return genericPool.GetFromPool(genericPool.sample);
    }
    SampleBoardPiece ExtractSquareUsing(int i, int j, int k)
    {
        SampleBoardPiece square = _board[i, j, k];
        _board[i, j, k] = null;
        return square;
    }
    #endregion //BOARD GENERATOR

    [ContextMenu(nameof(DestroyBoard))] void DestroyBoard() => DestroyBoard(_board);
    void DestroyBoard(SampleBoardPiece[,,] board)
    {
        Action<int, int, int> PushToPool = (i, j, k) =>
        {
            Pool<SampleBoardPiece> squarePool = GetPoolUsing(i + j + k);
            squarePool.PushToPool(board[i, j, k]);
        };
        MultidimentionalAction(board, PushToPool);
    }
    Pool<SampleBoardPiece> GetPoolUsing(int value) => IsPair(value) ? whitePool : blackPool;
    static void MultidimentionalAction(SampleBoardPiece[,,] board, Action<int, int, int> action)
    {
        for (int i = 0; i < board.GetLength(0); i++)
            for (int j = 0; j < board.GetLength(1); j++)
                for (int k = 0; k < board.GetLength(2); k++)
                    action(i, j, k);
    }
    void UpdatePadding(Vector3 padding)
    {
        Vector3 index = new Vector3();
        for (int i = 0; i < _board.GetLength(0); i++)
            for (int j = 0; j < _board.GetLength(1); j++)
                for (int k = 0; k < _board.GetLength(2); k++)
                {
                    index.Set(i, j, k);
                    SO_BoardSquare square = GetPoolUsing(i + j + k).sample.so_pieceData;
                    Vector3 squarePosition = MemberWiseMultiply(square.pieceBounds.size, index);
                    Vector3 squarePadding = MemberWiseMultiply(padding, index);
                    squarePosition += squarePadding + boardCenter;
                    _board[i, j, k].transform.position = squarePosition;
                }
        cachePadding = padding;
    }
    [ContextMenu(nameof(SetSquareRanks))]
    void SetSquareRanks()
    {
        for (int i = 0; i < _board.GetLength(0); i++)
            for (int j = 0; j < _board.GetLength(1); j++)
                for (int k = 0; k < _board.GetLength(2); k++)
                {
                    //TODO alterar script pra não precisar fazer GetComponent. Provavelmente, alterar o board de "GameObject" pra "SampleBoardPiece" resolva
                    SampleBoardPiece sampleBoardPiece = _board[i, j, k];
                    sampleBoardPiece.boardPosition = new Vector3Int(i, j, k);
                }
    }
    static bool IsPair(int value) => value % 2 == 0;
    static Vector3 MemberWiseMultiply(Vector3 a, Vector3 b) => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    #endregion //METHODS
}

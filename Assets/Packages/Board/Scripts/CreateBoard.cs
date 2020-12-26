using System;
using System.Collections;
using ExtensionMethods;
using UnityEngine;

public partial class CreateBoard : MonoBehaviour
{
    #region FIELDS
    public Vector3Int size;
    public Vector3 boardCenter;
    public Vector3 padding;
    public SampleBoardPiece[,,] board;
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

        if (DidSizeChanged(size)) ResetBoardSize(size);
        UpdatePadding(padding);
        SetSquareCoordenates();
    }
    [ContextMenu(nameof(ForceUpdateBoard))]
    public void ForceUpdateBoard() => ForceUpdateBoard(size, padding);
    public void ForceUpdateBoard(Vector3Int size, Vector3 padding)
    {
        ResetBoardSize(size);
        UpdatePadding(padding);
        SetSquareCoordenates();
    }
    [ContextMenu(nameof(PrintBoard))] void PrintBoard() { foreach (SampleBoardPiece square in board) Debug.Log(square, square); }

    #region CHANGE VERIFICATION
    bool DidValuesChange(Vector3Int size, Vector3 padding) => DidSizeChanged(size) || DidPaddingChanged(padding);
    bool DidSizeChanged(Vector3Int size) => size != cacheSize;
    bool DidPaddingChanged(Vector3 padding) => padding != cachePadding;
    #endregion //CHANGE VERIFICATION

    void ResetBoardSize(Vector3Int size)
    {
        if (board == null) InitializeBoard(size);
        SampleBoardPiece[,,] newBoard = GenerateBoardFromOld(size);
        DestroyBoard(board);
        board = newBoard;
        cacheSize = size;
    }
    SampleBoardPiece[,,] InitializeBoard(Vector3Int size) => board = InstantiateABoard(size);
    static SampleBoardPiece[,,] InstantiateABoard(Vector3Int size) => new SampleBoardPiece[size.x, size.y, size.z];

    #region -------- BOARD GENERATOR
    SampleBoardPiece[,,] GenerateBoardFromOld(Vector3Int size) => ResizeUsing(InstantiateABoard(size), board);
    SampleBoardPiece[,,] ResizeUsing(SampleBoardPiece[,,] newBoard, SampleBoardPiece[,,] board)
    {
        return newBoard.ForEachDoAction((i, j, k) =>
        {
            if (board.OutOfBounds(i, j, k) || board[i, j, k] == null)
                newBoard[i, j, k] = GenerateSquareUsing(i + j + k);
            else
                newBoard[i, j, k] = board.RemoveAt(i, j, k);
        });
    }
    SampleBoardPiece GenerateSquareUsing(int index)
    {
        Pool<SampleBoardPiece> genericPool = GetPool(index);
        return genericPool.GetFromPool(genericPool.sample);
    }
    #endregion //BOARD GENERATOR

    void DestroyBoard(SampleBoardPiece[,,] board)
    {
        Action<int, int, int> PushToPool = (i, j, k) =>
        {
            Pool<SampleBoardPiece> squarePool = GetPool(i + j + k);
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
                    Vector3 squareSize = this.GetPool(i + j + k).sample.so_pieceData.pieceBounds.size;
                    Vector3 squarePosition = squareSize.Scaled(index);
                    Vector3 squarePadding = padding.Scaled(index);

                    board[i, j, k].transform.position = squarePosition + squarePadding + boardCenter;
                }
        cachePadding = padding;
    }
    Pool<SampleBoardPiece> GetPool(int index) => IsPair(index) ? whitePool : blackPool;
    [ContextMenu(nameof(SetSquareCoordenates))]
    void SetSquareCoordenates() => board.ForEachDoAction(SetSquareCoordenates);
    void SetSquareCoordenates(int i, int j, int k) => board[i, j, k].BoardCoord = new Vector3Int(i, j, k);
    static bool IsPair(int value) => value % 2 == 0;
    static Vector3 MemberWiseMultiply(Vector3 a, Vector3 b) => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    #endregion //METHODS
}

using System;
using System.Collections;
using UnityEngine;

public partial class CreateBoard : MonoBehaviour
{
    #region FIELDS
    public Vector3Int size;
    public Vector3 boardCenter;
    public Vector3 padding;
    // public SO_BoardSquare whiteSquare;
    // public SO_BoardSquare blackSquare;
    public GameObject[,,] board;
    // public Transform boardPivot;
    public SquarePool whitePool;
    public SquarePool blackPool;
    [Serializable]
    public class SquarePool : Pool
    {
        public SO_BoardSquare squareData;
        public GameObject GetFromPool() => GetFromPool(squareData.prefab);
    }
    Vector3 cachePadding;
    Vector3 cacheSize;
    #endregion //FIELDS
    void Update() => TryUpdateBoard();

    #region METHODS
    [ContextMenu(nameof(TryUpdateBoard))] void TryUpdateBoard() => TryUpdateBoard(size, padding);
    public void TryUpdateBoard(Vector3Int size, Vector3 padding)
    {
        if (!DidValuesChange(size, padding)) return;

        if (IsSizeDiferent(size)) ResetBoardSize(size);
        UpdatePadding(padding);
    }
    [ContextMenu(nameof(ForceUpdateBoard))]
    public void ForceUpdateBoard() => ForceUpdateBoard(size, padding);
    public void ForceUpdateBoard(Vector3Int size, Vector3 padding)
    {
        ResetBoardSize(size);
        UpdatePadding(padding);
    }
    [ContextMenu(nameof(PrintBoard))] void PrintBoard() { foreach (GameObject square in board) Debug.Log(square, square); }
    #region CHANGE VERIFICATION
    bool DidValuesChange(Vector3Int size, Vector3 padding) => IsSizeDiferent(size) || IsPaddingDiferent(padding);
    bool IsSizeDiferent(Vector3Int size) => size != cacheSize;
    bool IsPaddingDiferent(Vector3 padding) => padding != cachePadding;
    #endregion //CHANGE VERIFICATION
    void ResetBoardSize(Vector3Int size)
    {
        if (board == null) InitializeBoard(size);
        GameObject[,,] newBoard = GenerateBoardFromOld(size);
        DestroyBoard();
        board = newBoard;
        cacheSize = size;
    }
    GameObject[,,] InitializeBoard(Vector3Int size) => board = InstantiateABoard(size);
    static GameObject[,,] InstantiateABoard(Vector3Int size) => new GameObject[size.x, size.y, size.z];
    GameObject[,,] GenerateBoardFromOld(Vector3Int size)
    {
        GameObject[,,] newBoard = InstantiateABoard(size);
        for (int i = 0; i < newBoard.GetLength(0); i++)
            for (int j = 0; j < newBoard.GetLength(1); j++)
                for (int k = 0; k < newBoard.GetLength(2); k++)
                {
                    if (i > board.GetUpperBound(0) || j > board.GetUpperBound(1) || k > board.GetUpperBound(2) || board[i, j, k] == null)
                        newBoard[i, j, k] = GenerateSquareUsing(i + j + k);
                    else
                        newBoard[i, j, k] = ExtractSquareUsing(i, j, k);
                }

        return newBoard;
    }
    GameObject GenerateSquareUsing(int index) => GetPoolUsing(index).GetFromPool();
    // IsPair(index) ? whitePool.GetFromPool() : blackPool.GetFromPool();
    GameObject ExtractSquareUsing(int i, int j, int k)
    {
        GameObject square = board[i, j, k];
        board[i, j, k] = null;
        return square;
    }
    [ContextMenu(nameof(DestroyBoard))] void DestroyBoard() => DestroyBoard(board);
    void DestroyBoard(GameObject[,,] board)
    {
        Action<int, int, int> PushToPool = (i, j, k) =>
        {
            SquarePool squarePool = GetPoolUsing(i + j + k);
            squarePool.PushToPool(board[i, j, k]);
        };
        MultidimentionalAction(board, PushToPool);
        // foreach (GameObject obj in board) if (obj != null) ReturnToPool(obj);
    }
    SquarePool GetPoolUsing(int value) => IsPair(value) ? whitePool : blackPool;
    static void MultidimentionalAction(GameObject[,,] board, Action<int, int, int> action)
    {
        for (int i = 0; i < board.GetLength(0); i++)
            for (int j = 0; j < board.GetLength(1); j++)
                for (int k = 0; k < board.GetLength(2); k++)
                    action(i, j, k);
    }
    void UpdatePadding(Vector3 padding)
    {
        Vector3 index = new Vector3();
        for (int i = 0; i < board.GetLength(0); i++)
            for (int j = 0; j < board.GetLength(1); j++)
                for (int k = 0; k < board.GetLength(2); k++)
                {
                    index.Set(i, j, k);
                    SO_BoardSquare square = GetPoolUsing(i + j + k).squareData;
                    Vector3 squarePosition = MemberWiseMultiply(square.pieceBounds.size, index);
                    Vector3 squarePadding = MemberWiseMultiply(padding, index);
                    squarePosition += squarePadding + boardCenter;
                    board[i, j, k].transform.position = squarePosition;
                }
        cachePadding = padding;
    }
    static bool IsPair(int value) => value % 2 == 0;
    static Vector3 MemberWiseMultiply(Vector3 a, Vector3 b) => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    #endregion //METHODS
}

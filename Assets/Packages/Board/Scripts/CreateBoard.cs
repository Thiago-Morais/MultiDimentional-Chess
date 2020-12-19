using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBoard : MonoBehaviour
{
    public Vector3Int size;
    public Vector3 center;
    public Vector3 padding;
    public SO_BoardPiece whiteSquare;
    public SO_BoardPiece blackSquare;
    public GameObject[,,] board;
    public Transform boardPivot;
    Vector3 cachePadding;
    Vector3 cacheSize;
    void Update() => UpdateBoard();

    [ContextMenu(nameof(UpdateBoard))]
    void UpdateBoard() => UpdateBoard(size);
    public void UpdateBoard(Vector3Int size)
    {
        if (size == cacheSize && padding == cachePadding) return;

        if (size != cacheSize)
        {
            ResetBoardSize(size);
            cacheSize = size;
        }
        if (padding != cachePadding)
        {
            UpdatePosition();
            cachePadding = padding;
        }
    }
    void UpdatePosition()
    {
        Vector3 index = new Vector3();
        for (int i = 0; i < board.GetLength(0); i++)
            for (int j = 0; j < board.GetLength(1); j++)
                for (int k = 0; k < board.GetLength(2); k++)
                {
                    index.Set(i, j, k);
                    SO_BoardPiece square = blackSquare;
                    GameObject squareObj = board[i, j, k];
                    Vector3 squarePosition = MemberWiseMultiply(square.pieceBounds.size, index);
                    Vector3 padding = MemberWiseMultiply(this.padding, index);
                    squarePosition += padding + center;
                    squareObj.transform.position = squarePosition;
                }
    }
    void ResetBoardSize(Vector3Int size)
    {
        if (board == null) InitializeBoard(size);
        GameObject[,,] newBoard = InstantiateBoard(size);
        for (int i = 0; i < newBoard.GetLength(0); i++)
            for (int j = 0; j < newBoard.GetLength(1); j++)
                for (int k = 0; k < newBoard.GetLength(2); k++)
                {
                    if (i > board.GetUpperBound(0) || j > board.GetUpperBound(1) || k > board.GetUpperBound(2) || board[i, j, k] == null)
                    {
                        SO_BoardPiece squareData = IsPair(i + j + k) ? blackSquare : whiteSquare;
                        newBoard[i, j, k] = Instantiate(squareData.prefab, boardPivot);
                    }
                    else
                    {
                        newBoard[i, j, k] = board[i, j, k];
                        board[i, j, k] = null;
                    }
                }
        DestroyBoard(board);
        board = newBoard;
    }
    static bool IsPair(int value) => value % 2 == 0;
    Vector3 MemberWiseMultiply(Vector3 a, Vector3 b) => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    void DestroyBoard(GameObject[,,] board) { foreach (GameObject obj in board) Destroy(obj); }
    void DestroyBoard() { foreach (GameObject obj in board) Destroy(obj); }
    GameObject[,,] InitializeBoard(Vector3Int size) => board = new GameObject[size.x, size.y, size.z];
    GameObject[,,] InstantiateBoard(Vector3Int size) => new GameObject[size.x, size.y, size.z];
    [ContextMenu(nameof(PrintBoard))]
    void PrintBoard()
    { foreach (GameObject go in board) Debug.Log(go, go); }
}

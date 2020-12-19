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
    [ContextMenu(nameof(UpdateBoard))]
    void UpdateBoard() => UpdateBoard(size);
    public void UpdateBoard(Vector3Int size)
    {
        if (size == cacheSize && padding == cachePadding) return;

        if (board == null) InitializeBoard(size);
        else DestroyBoard();

        Vector3 index = new Vector3();
        for (int i = 0; i < board.GetLength(0); i++)
            for (int j = 0; j < board.GetLength(1); j++)
                for (int k = 0; k < board.GetLength(2); k++)
                {
                    index.Set(i, j, k);
                    SO_BoardPiece square = blackSquare;
                    GameObject squareObj = square.prefab;
                    Vector3 squarePosition = MemberWiseMultiply(square.pieceBounds.size, index);
                    squarePosition += center;
                    Vector3 padding = MemberWiseMultiply(this.padding, index);
                    squarePosition += padding;

                    GameObject boardPiece = Instantiate(squareObj, squarePosition, squareObj.transform.rotation, boardPivot);
                    board[i, j, k] = boardPiece;
                }
        cacheSize = size;
        cachePadding = padding;
        PrintBoard();
    }
    Vector3 MemberWiseMultiply(Vector3 a, Vector3 b) => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    void DestroyBoard() { foreach (GameObject obj in board) Destroy(obj); }
    GameObject[,,] InitializeBoard(Vector3Int size) => board = new GameObject[size.x, size.y, size.z];
    [ContextMenu(nameof(PrintBoard))]
    void PrintBoard() { foreach (GameObject go in board) Debug.Log(go, go); }
}

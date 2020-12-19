using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBoard : MonoBehaviour
{
    public Vector3Int size;
    public Vector3 center;
    public SO_BoardPiece whiteSquare;
    public SO_BoardPiece blackSquare;
    public GameObject[,,] board;
    public Transform boardPivot;
    Vector3 cacheSize;
    [ContextMenu(nameof(UpdateBoard))]
    void UpdateBoard() => UpdateBoard(size);
    public void UpdateBoard(Vector3Int size)
    {
        if (size == cacheSize) return;

        if (board == null) InitializeBoard(size);
        else DestroyBoard();

        for (int i = 0; i < board.GetLength(0); i++)
            for (int j = 0; j < board.GetLength(1); j++)
                for (int k = 0; k < board.GetLength(2); k++)
                {
                    GameObject obj = board[i, j, k];
                    // GameObject boardPiece = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    SO_BoardPiece square = blackSquare;
                    GameObject squareObj = square.prefab;
                    float x = square.pieceBounds.size.x * i;
                    float y = square.pieceBounds.size.y * j;
                    float z = square.pieceBounds.size.z * k;
                    Vector3 position = new Vector3(x, y, z);
                    GameObject boardPiece = Instantiate(squareObj, position, squareObj.transform.rotation, boardPivot);
                    Debug.Log($"{nameof(i)} = {i.ToString("D2")}; {nameof(j)} = {j.ToString("D2")}; {nameof(k)} = {k.ToString("D2")}; ", boardPiece);
                    Debug.Log($"{nameof(position)} = {position}", boardPiece);
                    Debug.Log($"{nameof(square.pieceBounds.size)} = {square.pieceBounds.size}", boardPiece);
                    // boardPiece.transform.position = center + new Vector3(i, j, k);
                    board[i, j, k] = boardPiece;
                }
        cacheSize = size;
        PrintBoard();
    }
    void DestroyBoard() { foreach (GameObject obj in board) Destroy(obj); }
    GameObject[,,] InitializeBoard(Vector3Int size) => board = new GameObject[size.x, size.y, size.z];
    [ContextMenu(nameof(PrintBoard))]
    void PrintBoard() { foreach (GameObject go in board) Debug.Log(go, go); }
}

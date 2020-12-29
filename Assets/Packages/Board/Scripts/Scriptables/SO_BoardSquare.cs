using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = nameof(ScriptableObject) + "/Board/Piece", fileName = "new BoardPiece")]
public class SO_BoardSquare : ScriptableObject
{
    public GameObject prefab;
    // public BoardPiece boardPiece;
    public Bounds pieceBounds;
    public bool HavePrefab => prefab;
    public SO_BoardSquare() { }
    void Awake()
    {
        InitializeVariables();
        // UpdateSize();
    }
    [ContextMenu(nameof(InitializeVariables))]
    public void InitializeVariables()
    {
        if (!prefab) prefab = new GameObject();
        // if (!boardPiece) boardPiece = new GameObject().AddComponent<BoardPiece>();
        if (pieceBounds == default(Bounds)) pieceBounds = new Bounds();
    }
    // public static SO_BoardSquare CreateInstance(BoardPiece boardPiece)
    // {
    //     SO_BoardSquare so_BoardSquare = ScriptableObject.CreateInstance<SO_BoardSquare>();
    //     so_BoardSquare.boardPiece = boardPiece;
    //     return so_BoardSquare;
    // }
    [ContextMenu(nameof(UpdateSize))]
    public void UpdateSize(BoardPiece boardPiece)
    {
        // if (!HavePrefab)
        // {
        //     Debug.LogError($"{name} doesn't have prefab", this);
        //     return;
        // }

        // Renderer[] internalRenderers = prefab.GetComponentsInChildren<Renderer>();
        Renderer[] internalRenderers = boardPiece.GetComponentsInChildren<Renderer>();

        Bounds bounds = new Bounds();
        foreach (Renderer renderer in internalRenderers)
            bounds.Encapsulate(renderer.bounds);

        pieceBounds = bounds;
    }
}

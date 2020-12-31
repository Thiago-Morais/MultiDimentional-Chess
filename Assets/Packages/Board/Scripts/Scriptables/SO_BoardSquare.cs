using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = nameof(ScriptableObject) + "/Board/Piece", fileName = "new BoardPiece")]
public class SO_BoardSquare : ScriptableObject
{
    public GameObject prefab;
    public Bounds pieceBounds = new Bounds(Vector3.zero, Vector3.one);
    public bool HavePrefab => prefab;
    void Awake() => InitializeVariables();
    [ContextMenu(nameof(InitializeVariables))]
    public void InitializeVariables()
    {
        // if (!prefab) prefab = new GameObject("Prefab");
        if (pieceBounds == default(Bounds)) pieceBounds = new Bounds();
    }
    [ContextMenu(nameof(UpdateSize))]
    public void UpdateSize(BoardPiece boardPiece)
    {
        Renderer[] internalRenderers = boardPiece.GetComponentsInChildren<Renderer>();

        Bounds bounds = new Bounds();
        foreach (Renderer renderer in internalRenderers)
            bounds.Encapsulate(renderer.bounds);

        pieceBounds = bounds;
    }
}

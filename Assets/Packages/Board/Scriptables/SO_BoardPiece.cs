using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Board/Piece", fileName = "new BoardPiece")]
public class SO_BoardPiece : ScriptableObject
{
    public GameObject prefab;
    public Bounds pieceBounds;
    public bool HavePrefab => prefab;
    void Awake() => UpdateSize();

    [ContextMenu(nameof(UpdateSize))]
    public void UpdateSize()
    {
        if (!HavePrefab)
        {
            Debug.LogError($"{name} doesn't have prefab", this);
            return;
        }

        Renderer[] internalRenderers = prefab.GetComponentsInChildren<Renderer>();

        Bounds bounds = new Bounds();
        foreach (Renderer renderer in internalRenderers)
            bounds.Encapsulate(renderer.bounds);

        pieceBounds = bounds;
    }
}

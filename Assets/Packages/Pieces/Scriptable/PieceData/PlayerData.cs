using System;
using ExtensionMethods;
using UnityEngine;

[CreateAssetMenu(menuName = nameof(ScriptableObject) + "/" + nameof(PlayerData))]
public class PlayerData : ScriptableObject
{
    public bool isWhite;
    public Material piecesMaterial;
    public Vector3 forward;

    public void ApplyPlayerData(SamplePiece samplePiece)
    {
        SetMaterials(samplePiece);
        SetOrientation(samplePiece);
    }
    public void SetOrientation(SamplePiece samplePiece) => samplePiece.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    public void SetMaterials(SamplePiece samplePiece)
    {
        Renderer[] renderers = GetRendrers(samplePiece.gameObject);
        renderers.SetMaterials(piecesMaterial);
    }
    public Renderer[] GetRendrers(GameObject gameObject) => gameObject.GetComponentsInChildren<Renderer>();
}
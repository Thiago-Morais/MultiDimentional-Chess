using ExtensionMethods;
using UnityEngine;

[CreateAssetMenu(menuName = nameof(ScriptableObject) + "/" + nameof(PlayerData))]
public class PlayerData : ScriptableObject
{
    #region -------- FIELDS
    public bool isWhite = true;
    public Material piecesMaterial;
    public Vector3 forward = Vector3.forward;
    #endregion //FIELDS

    #region -------- PROPERTIES
    #endregion //PROPERTIES

    #region -------- OUTSIDE CALL
    #endregion //OUTSIDE CALL

    #region -------- METHODS
    public void ApplyPlayerData(Piece samplePiece)
    {
        SetMaterials(samplePiece);
        SetOrientation(samplePiece);
    }
    public void SetOrientation(Piece samplePiece) => samplePiece.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    public void SetMaterials(Piece samplePiece)
    {
        Renderer[] renderers = GetRendrers(samplePiece.gameObject);
        renderers.SetMaterials(piecesMaterial);
    }
    public Renderer[] GetRendrers(GameObject gameObject) => gameObject.GetComponentsInChildren<Renderer>();
    #endregion //METHODS
}
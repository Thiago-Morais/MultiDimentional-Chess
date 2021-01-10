using UnityEngine;

public class BoardPiece : MonoBehaviour, IInitializable, IPoolable, IHighlightable, ISelectable, IMediator<BoardPiece.IntFlags>, IOnBoard
{
    #region -------- FIELDS
    public SO_BoardSquare so_pieceData;
    [SerializeField] Highlight highlight;
    public Transform pieceTarget;
    [SerializeField] Vector3Int boardCoord;
    public Piece currentPiece;
    #endregion //FIELDS

    #region -------- PROPERTIES
    public Vector3Int BoardCoord { get => boardCoord; set => boardCoord = value; }        //TODO setar posição do square no inicio
    public Highlight Highlight { get => highlight; set => highlight = value; }
    #endregion //PROPERTIES

    void Awake()
    {
        InitializeVariables();
        SignOn();
    }
    public void InitializeVariables()
    {
        if (!Highlight)
        {
            Highlight = GetComponentInChildren<Highlight>();
            if (!Highlight)
            {
                Highlight = gameObject.AddComponent<Highlight>().Initialized() as Highlight;
            }
        }
        if (!so_pieceData) so_pieceData = ScriptableObject.CreateInstance<SO_BoardSquare>();
        if (!pieceTarget)
        {
            pieceTarget = new GameObject(nameof(pieceTarget)).transform;
            pieceTarget.SetParent(transform);
        }
    }
    public enum IntFlags
    {
        none = 0,
        Selected = 1 << 0,
        Deselected = 1 << 1,
    }

    #region -------- METHODS
    #region -------- MEDIATOR
    public void SignOn() => ContextMediator.SignOn(this);
    public void Notify(IntFlags intFlag) => ContextMediator.Notify(this, intFlag);
    #endregion //MEDIATOR

    public void OnSelected()
    {
        Notify(IntFlags.Selected);
    }
    public void OnDeselected()
    {
        Notify(IntFlags.Deselected);
        Highlight.ClearHighlight();
    }
    [ContextMenu(nameof(UpdateSize))]
    public void UpdateSize() => so_pieceData.UpdateSize(this);
    public Component Deactivated()
    {
        gameObject.SetActive(false);
        return this;
    }
    public Component Activated()
    {
        gameObject.SetActive(true);
        return this;
    }
    public Component InstantiatePoolable() => Instantiate(this);
    public IInitializable Initialized(Transform parent = null)
    {
        InitializeVariables();
        transform.parent = parent;
        return this;
    }
    #endregion //METHODS
}

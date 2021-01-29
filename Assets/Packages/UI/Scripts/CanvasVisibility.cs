
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public class CanvasVisibility : MonoBehaviour, IInitializable
{
    #region -------- FIELDS
    [SerializeField] CanvasGroup canvasGroup;
    #endregion //FIELDS

    #region -------- PROPERTIES
    public CanvasGroup CanvasGroup { get => canvasGroup; private set => canvasGroup = value; }
    #endregion //PROPERTIES

    #region -------- OUTSIDE CALL
    void Awake() => InitializeVariables();
    #endregion //OUTSIDE CALL

    #region -------- METHODS
    [ContextMenu(nameof(SetCanvasGroupOn))] public void SetCanvasGroupOn() => SetCanvasGroup(true);
    [ContextMenu(nameof(SetCanvasGroupOff))] public void SetCanvasGroupOff() => SetCanvasGroup(false);
    public void SetCanvasGroup(bool visible)
    {
        CanvasGroup.blocksRaycasts = visible;
        CanvasGroup.alpha = visible ? 1 : 0;
    }
    public IInitializable Initialized(Transform parent = null)
    {
        InitializeVariables();
        return this;
    }
    [ContextMenu(nameof(InitializeVariables))]
    void InitializeVariables() { if (!CanvasGroup) CanvasGroup = gameObject.GetComponent<CanvasGroup>(); }
    #endregion //METHODS
}
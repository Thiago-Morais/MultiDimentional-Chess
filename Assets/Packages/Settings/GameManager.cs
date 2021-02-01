using System;
using UnityEngine;
using static ExtensionMethods.ReferenceExtension;
using CustomUI;

public class GameManager : MonoBehaviour, IInitializable
{
    #region -------- FIELDS
    public GameSettings settings;
    public PlayerColor playerTurn;
    public PlayerData currentPlayer = default;
    public HoverControll hoverControll;
    public FloatEvent notifyHoverSensitivity = new FloatEvent();
    #endregion //FIELDS

    #region -------- PROPERTIES
    public static GameManager Instance { get; private set; }
    #endregion //PROPERTIES

    #region -------- OUTSIDE CALL
    void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(this);

        SignOn();
    }

    void Start()
    {
        UpdateHoverSensitivity();
    }
    #endregion //OUTSIDE CALL

    #region -------- METHODS
    public void TogglePlayer()
    {
        if (playerTurn == PlayerColor.white)
            playerTurn = PlayerColor.black;
        else
            playerTurn = PlayerColor.white;
    }
    public bool IsTurn(PlayerData playerData) => currentPlayer == playerData;
    public void SetHoverSensitivity(float hoverSensitivity)
    {
        settings.hoverSensitivity = hoverSensitivity;
        UpdateHoverSensitivity();
    }
    public void UpdateHoverSensitivity()
    {
        hoverControll.HoverSensitivity = settings.hoverSensitivity;
        NotifyHoverSensitivity();
    }
    void NotifyHoverSensitivity() => notifyHoverSensitivity.Invoke(settings.hoverSensitivity);
    void InitializeVariables()
    {
        if (!settings) settings = ScriptableObject.CreateInstance<GameSettings>();
        if (!hoverControll) hoverControll = InstantiateInitialized<HoverControll>();
    }
    #region -------- INTERFACE
    #region -------- MEDIATOR
    public void SignOn() => ContextMediator.SignOn(this);
    public void Notify(IntFlags intFlag) => ContextMediator.Notify(this, intFlag);
    #endregion //MEDIATOR
    public IInitializable Initialized(Transform parent = null)
    {
        InitializeVariables();
        return this;
    }
    #endregion //INTERFACE
    #endregion //METHODS

    #region -------- ENUM
    [Flags]
    public enum IntFlags
    {
        none = 0,
    }
    #endregion //ENUM
}

using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region -------- FIELDS
    public PlayerColor playerTurn;
    public PlayerData currentPlayer = default;
    public static GameManager Instance { get; private set; }
    #endregion //FIELDS

    #region -------- PROPERTIES
    #endregion //PROPERTIES

    #region -------- OUTSIDE CALL
    void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(this);

        SignOn();
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
    public void SignOn() => ContextMediator.SignOn(this);
    public void Notify(IntFlags intFlag) => ContextMediator.Notify(this, intFlag);
    #endregion //METHODS

    #region -------- ENUM
    [Flags]
    public enum IntFlags
    {
        none = 0,
    }
    #endregion //ENUM
}

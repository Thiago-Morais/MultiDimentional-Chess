using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour/* , IMediatorInstance<GameManager, GameManager.IntFlags> */
{
    #region -------- FIELDS
    public PlayerColor playerTurn;
    public PlayerData currentPlayer = default;
    public static GameManager Instance { get; private set; }

    // MediatorConcrete<GameManager, IntFlags> mediator = new MediatorConcrete<GameManager, IntFlags>();
    #endregion //FIELDS

    #region -------- PROPERTIES
    // public MediatorConcrete<GameManager, IntFlags> Mediator { get => mediator; set => mediator = value; }
    #endregion //PROPERTIES

    [Flags]
    public enum IntFlags
    {
        none = 0,
    }
    void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(this);

        SignOn();
    }
    #region -------- METHODS
    public void TogglePlayer()
    {
        if (playerTurn == PlayerColor.white)
            playerTurn = PlayerColor.black;
        else
            playerTurn = PlayerColor.white;
    }
    public bool IsTurn(PlayerData playerData) => currentPlayer == playerData;
    // public void SignOn() => Mediator.SignOn(this);
    // public void Notify(IntFlags intFlag) => Mediator.Notify(this, intFlag);
    public void SignOn() => ContextMediator.SignOn(this);
    public void Notify(IntFlags intFlag) => ContextMediator.Notify(this, intFlag);
    #endregion //METHODS
}

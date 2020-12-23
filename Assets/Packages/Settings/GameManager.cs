using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region -------- FIELDS
    public PlayerColor playerTurn;
    #endregion //FIELDS
    #region -------- PROPERTIES
    #endregion //PROPERTIES
    #region -------- METHODS
    public void TogglePlayer()
    {
        if (playerTurn == PlayerColor.white)
            playerTurn = PlayerColor.black;
        else
            playerTurn = PlayerColor.white;
    }
    #endregion //METHODS
}

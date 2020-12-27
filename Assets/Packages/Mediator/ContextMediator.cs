using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif //UNITY_EDITOR

public class ContextMediator
{
#if UNITY_EDITOR
    [MenuItem("MyMenu/" + nameof(PrintElements))]
    public static void PrintElements()
    {
        if (pieces != null)
        {
            foreach (var item in pieces)
                Debug.Log($"{nameof(Piece)} - {item.name}", item.gameObject);
        }
        if (squares != null)
        {
            foreach (var item in squares)
                Debug.Log($"{nameof(BoardPiece)} - {item.name}", item.gameObject);
        }
    }
#endif //UNITY_EDITOR
    static List<Piece> pieces = new List<Piece>();
    static List<BoardPiece> squares = new List<BoardPiece>();
    static DinamicBoard dinamicBoard;
    static Selector selector;
    static GameManager gameManager;

    // public static void SignOn(MonoBehaviour sender)
    // {
    //     switch (sender)
    //     {
    //         case Piece c: SignOn(sender as Piece); break;
    //         case BoardPiece c: SignOn(sender as BoardPiece); break;
    //         case DinamicBoard c: SignOn(sender as DinamicBoard); break;
    //         case Selector c: SignOn(sender as Selector); break;
    //     }
    // }
    // public static void Notify(MonoBehaviour sender, Enum action)
    // {
    //     switch (sender)
    //     {
    //         case Piece c: Notify(sender as Piece, action); break;
    //         case BoardPiece c: Notify(sender as BoardPiece, action); break;
    //         case DinamicBoard c: Notify(sender as DinamicBoard, action); break;
    //         case Selector c: Notify(sender as Selector, action); break;
    //     }
    // }
    public static void SignOn(GameManager sender) => gameManager = sender;
    public static void Notify(GameManager sender, Enum action)
    {
    }
    public static void SignOn(Piece sender) => pieces.Add(sender);
    public static void Notify(Piece sender, Enum action)
    {
        switch (action)
        {
            case Piece.IntFlags.Selected:
                sender.highlight.SetHighlightOn(true);
                foreach (BoardPiece square in squares)
                    if (sender.BoardCoord == square.BoardCoord)
                    {
                        square.highlight.HighlightOn(Highlight.HighlightType.selected);
                    }
                    else if (sender.IsMovimentAvailable(square, sender.captureSet))
                    {
                        square.highlight.HighlightOn(Highlight.HighlightType.capturable);
                    }
                    else if (sender.IsMovimentAvailable(square, sender.moveSet))
                    {
                        square.highlight.HighlightOn(Highlight.HighlightType.movable);
                    }
                    else
                    {
                        square.highlight.HighlightOff();
                    }
                break;
            case Piece.IntFlags.UpdateTarget:
                // Vector3Int boardCoord = sender.BoardCoord;
                // BoardPiece boardPiece = dinamicBoard.GetSquareAt(boardCoord);
                // if (!boardPiece) { Debug.LogError($"Tryed to get Square at {boardCoord} but it was out of bounds.", sender); break; }

                // sender.targetSquare = boardPiece;
                break;
            case Piece.IntFlags.MoveToCoord:
                BoardPiece boardPiece = dinamicBoard.GetSquareAt(sender.BoardCoord);
                if (!boardPiece) { Debug.LogError($"Tryed to get Square at {sender.BoardCoord} but it was out of bounds.", sender); break; }

                sender.MoveTo(boardPiece);
                break;
            default: break;
        }
    }
    public static void SignOn(BoardPiece sender) => squares.Add(sender);
    public static void Notify(BoardPiece sender, Enum action)
    {
        switch (action)
        {
            case BoardPiece.IntFlags.Selected:
                Piece selectedPiece = selector.currentSelected as Piece;
                if (!selectedPiece) break;

                if (selectedPiece.IsAnyMovimentAvailable(sender))
                {
                    selectedPiece.BoardCoord = sender.BoardCoord;
                    selectedPiece.MoveToCoord();
                }
                foreach (BoardPiece square in squares) square.OnDeselected();
                break;
            default: break;
        }
    }
    public static void SignOn(DinamicBoard sender) => dinamicBoard = sender;
    public static void Notify(DinamicBoard sender, Enum action)
    {
    }
    public static void SignOn(Selector sender) => selector = sender;
    public static void Notify(Selector sender, Enum action)
    {
        switch (action)
        {
            case Selector.IntFlags.SelectionChanged:
                /*
                verifica se foi clicado em:
                    nada
                    um quadrado vazio
                    um quadrado com uma peça
                        se essa peça é aliada
                        se essa peça é inimiga
                */
                // Type type = sender.currentSelected.GetType();
                // if (type == typeof(Piece))
                // {
                //     Piece selectedPiece = selector.currentSelected as Piece;
                // }
                // else if (type == typeof(BoardPiece))
                //     if (!selectedPiece) break;

                // if (selectedPiece.IsAnyMovimentAvailable(sender))
                // {
                //     selectedPiece.BoardCoord = sender.BoardCoord;
                //     selectedPiece.MoveToCoord();
                // }
                break;
            case Selector.IntFlags.DeselectAll:
                foreach (BoardPiece square in squares) square.OnDeselected();
                foreach (Piece piece in pieces) piece.OnDeselected();
                break;
            default: break;
        }
    }
}

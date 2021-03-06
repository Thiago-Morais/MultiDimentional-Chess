﻿using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
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
    public static void SignOn(GameManager sender) => gameManager = sender;
    public static void Notify(GameManager sender, Enum action) { }
    public static void SignOn(Piece sender) => pieces.Add(sender);
    public static void Notify(Piece sender, Enum action)
    {
        switch (action)
        {
            case Piece.IntFlags.ShowPossibleMoves:
                if (dinamicBoard)
                    foreach (BoardPiece square in dinamicBoard?.board)
                    {
                        HighlightType highlightType = HighlightType.none;
                        if (sender.BoardCoord == square.BoardCoord)
                        {
                            highlightType = HighlightType.selected;
                        }
                        else if (sender.IsCaptureAvailable(square))
                        {
                            highlightType = HighlightType.capturable;
                        }
                        else if (sender.IsMovementAvailable(square))
                        {
                            highlightType = HighlightType.movable;
                        }
                        else
                        {
                            square.Highlight.ClearHighlight();
                            continue;
                        }
                        square.Highlight.HighlightOn(highlightType);
                    }
                break;
            case Piece.IntFlags.HidePossibleMoves:
                if (dinamicBoard)
                    foreach (BoardPiece square in dinamicBoard?.board)
                        square.Highlight.ClearHighlight();
                break;
            case Piece.IntFlags.MoveToCoord:
                BoardPiece boardPiece = dinamicBoard.GetSquareAt(sender.BoardCoord);
                if (!boardPiece) { Debug.LogError($"Tried to get Square at {sender.BoardCoord} but it was out of bounds.", sender); break; }

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

                if (selectedPiece.IsAnyMovementAvailable(sender))
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
    public static void Notify(DinamicBoard sender, Enum action) { }
    public static void SignOn(Selector sender) => selector = sender;
    public static void Notify(Selector sender, Enum action)
    {
        switch (action)
        {
            case Selector.IntFlags.SelectionChanged:
                break;
            case Selector.IntFlags.DeselectAll:
                foreach (BoardPiece square in squares) square.OnDeselected();
                foreach (Piece piece in pieces) piece.OnDeselected();
                break;
            default: break;
        }
    }
}

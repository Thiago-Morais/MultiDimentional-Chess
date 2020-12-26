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
                Debug.Log($"{nameof(SamplePiece)} - {item.name}", item.gameObject);
        }
        if (squares != null)
        {
            foreach (var item in squares)
                Debug.Log($"{nameof(BoardPiece)} - {item.name}", item.gameObject);
        }
    }
#endif //UNITY_EDITOR
    static List<SamplePiece> pieces = new List<SamplePiece>();
    static List<BoardPiece> squares = new List<BoardPiece>();
    static DinamicBoard dinamicBoard;
    static Selector selector;
    public static void SignOn(SamplePiece sender) => pieces.Add(sender);
    public static void Notify(SamplePiece sender, Enum action)
    {
        switch (action)
        {
            case SamplePiece.IntFlags.Selected:
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
            case SamplePiece.IntFlags.UpdateTarget:
                Vector3Int boardCoord = sender.BoardCoord;
                BoardPiece boardPiece = dinamicBoard.GetSquareAt(boardCoord);
                if (!boardPiece) { Debug.LogError($"Tryed to get Square at {boardCoord} but it was out of bounds.", sender); break; }

                sender.targetSquare = boardPiece;
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
                /*
                verifica se ele pode ser movido
                se for, move
                deseleciona
                */
                SamplePiece selectedPiece = (SamplePiece)selector.currentSelected;
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
    }
}

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
                Debug.Log($"{nameof(SampleBoardPiece)} - {item.name}", item.gameObject);
        }
    }
#endif //UNITY_EDITOR
    public static List<SamplePiece> pieces = new List<SamplePiece>();
    public static List<SampleBoardPiece> squares = new List<SampleBoardPiece>();
    public static void SignOn(SamplePiece sender) => pieces.Add(sender);
    public static void Notify(SamplePiece sender, Enum action)
    {
        switch (action)
        {
            case SamplePiece.IntFlags.Selected:
                foreach (SampleBoardPiece square in squares)
                    if (sender.boardPosition == square.boardPosition)
                    {
                        square.highlight.HighlightOn(Highlight.HighlightType.selected);
                    }
                    else if (sender.IsMoveAvailable(square, sender.captureSet))
                    {
                        square.highlight.HighlightOn(Highlight.HighlightType.capturable);
                    }
                    else if (sender.IsMoveAvailable(square, sender.moveSet))
                    {
                        square.highlight.HighlightOn(Highlight.HighlightType.movable);
                    }
                    else
                    {
                        square.highlight.HighlightOff();
                    }
                break;
            default: break;
        }
    }
    public static void SignOn(SampleBoardPiece sender) => squares.Add(sender);
    public static void Notify(SampleBoardPiece sender, Enum action)
    {
    }

}

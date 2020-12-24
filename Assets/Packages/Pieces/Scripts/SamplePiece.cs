using System;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public class SamplePiece : MonoBehaviour, ISelectable, IMediator<SamplePiece, SamplePiece.IntFlags>, IHighlightable
{
    #region -------- FIELDS
    public SampleBoardPiece targetSquare;
    public SampleBoardPiece currentSquare;
    public Transform targetTransform;
    public Highlight highlight;
    public IntFlags intFlags;
    public Vector3Int boardPosition;
    public PieceMoveSet moveSet;
    #endregion //FIELDS
    #region -------- PROPERTIES
    public bool Selected
    {
        get => intFlags.HasAny(IntFlags.Selected);
        set
        {
            if (value) intFlags = intFlags.Include(IntFlags.Selected);
            else intFlags = intFlags.Exclude(IntFlags.Selected);
        }
    }
    public Highlight Highlight { get => highlight; set => highlight = value; }
    #endregion //PROPERTIES
    [Flags]
    public enum IntFlags
    {
        none = 0,
        Selected = 1 << 0,
    }
    void Awake()
    {
        InitializeVariables();
        SignOn(this);
    }
    void InitializeVariables()
    {
        if (!highlight) highlight = GetComponent<Highlight>();
    }
    #region -------- METHODS
    #region -------- MEDIATOR
    public void SignOn(SamplePiece sender)
    {
        ContextMediator.SignOn(sender);
    }
    public void Notify(SamplePiece sender, IntFlags intFlag)
    {
        ContextMediator.Notify(sender, intFlag);
    }
    #endregion //MEDIATOR
    [ContextMenu(nameof(MoveUsingTransform))]
    public void MoveUsingTransform() => MoveTo(targetTransform);
    public void MoveTo(Transform target)
    {
        transform.position = target.position;
    }
    [ContextMenu(nameof(MoveUsingSO))]
    public void MoveUsingSO() => MoveTo(targetSquare);
    public void MoveTo(SampleBoardPiece target)
    {
        transform.position = target.pieceTarget.position;
        currentSquare = target;
        boardPosition = target.boardPosition;
    }
    public void OnSelected()
    {
        // highlight.LockHighlight();
        Notify(this, IntFlags.Selected);
    }
    public void OnDeselected()
    {
        // highlight.UnlockHighlight();
    }
    public bool IsMoveAvailable(SampleBoardPiece square)
    {
        Vector3Int dif = square.boardPosition - boardPosition;

        return moveSet.IsMoveAvailable(dif);
    }
    #endregion //METHODS
}

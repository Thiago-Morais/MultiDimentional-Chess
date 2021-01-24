using System;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using System.Linq;

[CreateAssetMenu(menuName = nameof(ScriptableObject) + "/" + nameof(PieceMoveSet))]
public class PieceMoveSet : ScriptableObject
{
    #region -------- FIELDS
    public Dimentions maxDimentionsAmount = Dimentions.all;
    public Dimentions dimentionalBinding = Dimentions.none;
    [Tooltip("Number of squares this piece can move on this dimention.\n" + "0 means no limit.")]
    public LimitType distanceLimitType = LimitType.atMostAll;
    public List<int> distanceLimitPerDimention = new List<int>();
    public Dimentions lockedDimentions = Dimentions.none;
    public Dimentions lockedBackwards = Dimentions.none;
    public Dimentions rayBlocked = Dimentions.all;
    #endregion //FIELDS

    #region -------- PROPERTIES
    #endregion //PROPERTIES

    #region -------- OUTSIDE CALL
    #endregion //OUTSIDE CALL

    #region -------- METHODS
    // public bool IsMovimentAvailable(Vector3Int direction, bool isWhite)       //TODO test it
    public bool IsMovimentAvailable(Piece piece, BoardPiece square)       //TODO test it
    {
        Vector3Int offset = piece.VectorDifference(square);

        if (IsRayBlocked(piece, square)
            || HasLockedDirection(offset)
            || HasLockedBackwards(offset, !piece.playerData.isWhite)
            || !IsWithinMaxDimentionsAmount(offset)
            || !IsWithinDimentionalBinding(offset)
            || !IsWithinMovimentLimits(offset)
            || IsOwnPosition(offset)
            )
            return false;
        return true;
    }
    public bool HasLockedDirection(Vector3Int direction)
    {
        if (lockedDimentions == Dimentions.none) return false;
        if (lockedDimentions.HasAny(Dimentions.one) && direction.x != 0) return true;
        if (lockedDimentions.HasAny(Dimentions.two) && direction.z != 0) return true;
        if (lockedDimentions.HasAny(Dimentions.three) && direction.y != 0) return true;
        return false;
    }
    public bool HasLockedBackwards(Vector3Int direction, bool inversed)
    {
        if (inversed) direction = -direction;
        if (lockedBackwards.HasAny(Dimentions.one) && direction.x < 0) return true;
        if (lockedBackwards.HasAny(Dimentions.two) && direction.z < 0) return true;
        if (lockedBackwards.HasAny(Dimentions.three) && direction.y < 0) return true;
        return false;
    }
    #region -------- DIMENTIONAL LIMITS
    public bool IsWithinMaxDimentionsAmount(Vector3Int direction)
    {
        Dimentions directions = direction.Rank().RankAs<Dimentions>();
        return maxDimentionsAmount.HasAny(directions);
    }      //TODO test it
    #endregion //DIMENTIONAL LIMITS

    #region -------- DIMENTIONAL BINDINGS 
    public bool IsWithinDimentionalBinding(Vector3Int direction)      //TODO test it
    {
        List<int> binds = DimentionalBinds();
        if (binds.Count == 0) return true;

        Dimentions dimentionalLimits = direction.Rank().RankAs<Dimentions>();
        if (!dimentionalBinding.HasAny(dimentionalLimits)) return false;

        List<int> limits = direction.AsList();
        if (dimentionalLimits == Dimentions.two) return HasBindedLimits(limits, 2);
        if (dimentionalLimits == Dimentions.three) return HasBindedLimits(limits, 3);
        return true;
    }
    public List<int> DimentionalBinds() => DimentionalBinds(dimentionalBinding);        //TODO test it
    public static List<int> DimentionalBinds(Dimentions dimentionalBinding)     //TODO test it
    {
        List<int> bind = new List<int>();
        if (dimentionalBinding.HasAny(Dimentions.one)) bind.Add(1);
        if (dimentionalBinding.HasAny(Dimentions.two)) bind.Add(2);
        if (dimentionalBinding.HasAny(Dimentions.three)) bind.Add(3);
        return bind;
    }
    public static bool HasBindedLimits(List<int> limits, int bindingAmount)     //TODO test it
    {
        var groupedLimits = limits.GroupBy(limit => Mathf.Abs(limit));
        var binded = groupedLimits.Where(group => group.Key != 0 && group.Count() >= bindingAmount);
        return binded.Count() > 0;
    }
    #endregion //DIMENTIONAL BINDINGS 

    #region -------- MOVIMENT LIMITS
    public bool IsWithinMovimentLimits(Vector3Int direction)      //TODO test it
    {
        if (distanceLimitPerDimention.Count == 0) return true;
        List<int> _direction = direction.AbsolutesOverZero();
        return IsWithinMovimentLimits(_direction);
    }
    public bool IsWithinMovimentLimits(List<int> direction)       //TODO test it
    {
        List<int> limitsCache = new List<int>(distanceLimitPerDimention);
        List<int> directionCache = new List<int>(direction);
        //TODO otimizar essa verificação (só precisa retornar a quantidade de elementos que não deram match)
        limitsCache.RemoveMatchingElements(directionCache);
        switch (distanceLimitType)
        {
            case LimitType.atMostAll: return directionCache.Count == 0;
            case LimitType.atLeastAll: return limitsCache.Count == 0;
            case LimitType.all: return limitsCache.Count == 0 && directionCache.Count == 0;
            default: return true;
        }
    }
    #endregion //MOVIMENT LIMITS
    public static bool IsOwnPosition(Vector3Int direction) => direction == Vector3Int.zero;
    public bool IsRayBlocked(Piece piece, BoardPiece square)
    {
        if (rayBlocked == Dimentions.none) return false;

        Vector3Int direction = piece.VectorDifference(square);
        Dimentions rankAsDimentions = direction.Rank().RankAs<Dimentions>();
        if (rayBlocked.HasAny(rankAsDimentions))
            return square.board.HasPieceBetween(piece.BoardCoord, square.BoardCoord);

        return false;
        // Vector3Int direction = piece.VectorDifference(square);
        // if (direction.Rank() > 1)
        // {
        //     if (direction.x != 0 && direction.y != 0 && Mathf.Abs(direction.x) != Mathf.Abs(direction.y)) return false;
        //     if (direction.x != 0 && direction.z != 0 && Mathf.Abs(direction.x) != Mathf.Abs(direction.z)) return false;
        //     if (direction.y != 0 && direction.z != 0 && Mathf.Abs(direction.y) != Mathf.Abs(direction.z)) return false;
        // }

        // return true;
    }
    #endregion //METHODS
}
[Flags]
public enum Dimentions
{
    none = 0,
    one = 1 << 0,
    two = 1 << 1,
    three = 1 << 2,
    all = ~0
}
public enum LimitType
{
    none,
    atMostAll,
    atLeastAll,
    all,
}

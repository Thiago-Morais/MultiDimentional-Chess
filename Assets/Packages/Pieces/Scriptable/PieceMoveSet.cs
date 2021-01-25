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
    public Dimentions dimensionalBinding = Dimentions.none;
    [Tooltip("Number of squares this piece can move on this dimension.\n" + "0 means no limit.")]
    public LimitType distanceLimitType = LimitType.atMostAll;
    public List<int> distanceLimitPerDimension = new List<int>();
    public Dimentions lockedDimentions = Dimentions.none;
    public Dimentions lockedBackwards = Dimentions.none;
    public Dimentions rayBlocked = Dimentions.all;
    #endregion //FIELDS

    #region -------- PROPERTIES
    #endregion //PROPERTIES

    #region -------- OUTSIDE CALL
    #endregion //OUTSIDE CALL

    #region -------- METHODS
    // public bool IsMovementAvailable(Vector3Int direction, bool isWhite)       //TODO test it
    public bool IsMovementAvailable(Piece piece, BoardPiece square)       //TODO test it
    {
        Vector3Int offset = piece.VectorDifference(square);

        if (IsRayBlocked(piece, square)
            || HasLockedDirection(offset)
            || HasLockedBackwards(offset, !piece.playerData.isWhite)
            || !IsWithinMaxDimentionsAmount(offset)
            || !IsWithinDimensionalBinding(offset)
            || !IsWithinMovementLimits(offset)
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
    public bool HasLockedBackwards(Vector3Int direction, bool inverted)
    {
        if (inverted) direction = -direction;
        if (lockedBackwards.HasAny(Dimentions.one) && direction.x < 0) return true;
        if (lockedBackwards.HasAny(Dimentions.two) && direction.z < 0) return true;
        if (lockedBackwards.HasAny(Dimentions.three) && direction.y < 0) return true;
        return false;
    }
    #region -------- DIMENSIONAL LIMITS
    public bool IsWithinMaxDimentionsAmount(Vector3Int direction)
    {
        Dimentions directions = direction.Rank().RankAs<Dimentions>();
        return maxDimentionsAmount.HasAny(directions);
    }      //TODO test it
    #endregion //DIMENSIONAL LIMITS

    #region -------- DIMENSIONAL BINDINGS 
    public bool IsWithinDimensionalBinding(Vector3Int direction)      //TODO test it
    {
        List<int> binds = DimensionalBinds();
        if (binds.Count == 0) return true;

        Dimentions dimensionalLimits = direction.Rank().RankAs<Dimentions>();
        if (!dimensionalBinding.HasAny(dimensionalLimits)) return false;

        List<int> limits = direction.AsList();
        if (dimensionalLimits == Dimentions.two) return HasBindedLimits(limits, 2);
        if (dimensionalLimits == Dimentions.three) return HasBindedLimits(limits, 3);
        return true;
    }
    public List<int> DimensionalBinds() => DimensionalBinds(dimensionalBinding);        //TODO test it
    public static List<int> DimensionalBinds(Dimentions dimensionalBinding)     //TODO test it
    {
        List<int> bind = new List<int>();
        if (dimensionalBinding.HasAny(Dimentions.one)) bind.Add(1);
        if (dimensionalBinding.HasAny(Dimentions.two)) bind.Add(2);
        if (dimensionalBinding.HasAny(Dimentions.three)) bind.Add(3);
        return bind;
    }
    public static bool HasBindedLimits(List<int> limits, int bindingAmount)     //TODO test it
    {
        var groupedLimits = limits.GroupBy(limit => Mathf.Abs(limit));
        var binded = groupedLimits.Where(group => group.Key != 0 && group.Count() >= bindingAmount);
        return binded.Count() > 0;
    }
    #endregion //DIMENSIONAL BINDINGS 

    #region -------- MOVEMENT LIMITS
    public bool IsWithinMovementLimits(Vector3Int direction)      //TODO test it
    {
        if (distanceLimitPerDimension.Count == 0) return true;
        List<int> _direction = direction.AbsolutesOverZero();
        return IsWithinMovementLimits(_direction);
    }
    public bool IsWithinMovementLimits(List<int> direction)       //TODO test it
    {
        List<int> limitsCache = new List<int>(distanceLimitPerDimension);
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
    #endregion //MOVEMENT LIMITS
    public static bool IsOwnPosition(Vector3Int direction) => direction == Vector3Int.zero;
    public bool IsRayBlocked(Piece piece, BoardPiece square)
    {
        if (rayBlocked == Dimentions.none) return false;

        Vector3Int direction = piece.VectorDifference(square);
        Dimentions rankAsDimentions = direction.Rank().RankAs<Dimentions>();
        if (rayBlocked.HasAny(rankAsDimentions))
            return square.board.HasPieceBetween(piece.BoardCoord, square.BoardCoord);

        return false;
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

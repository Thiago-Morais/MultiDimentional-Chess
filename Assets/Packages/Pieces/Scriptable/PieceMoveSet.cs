using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using ExtensionMethods;
using System.Linq;
using UnityEngine.Animations;

[CreateAssetMenu(menuName = nameof(ScriptableObject) + "/" + nameof(PieceMoveSet))]
public class PieceMoveSet : ScriptableObject
{
    public Dimentions dimentionalLimits = Dimentions.all;
    public Dimentions dimentionalBinding = Dimentions.none;
    [Tooltip("Number of squares this piece can move on this dimention.\n" + "0 means no limit.")]
    public LimitType limitType = LimitType.atMostAll;
    public List<int> movimentLimits = new List<int>();
    public Dimentions blockedDimentions = Dimentions.none;
    public Axis backwardsBlocker = Axis.None;

    public bool IsMovimentAvailable(Vector3Int direction, bool isWhite)       //TODO test it
    {
        if (IsDimentionBlocked(direction)
            || IsBackwardsBlocked(direction, isWhite)
            || !IsWithinDimentionalLimits(direction)
            || !IsWithinDimentionalBinding(direction)
            || !IsWithinMovimentLimits(direction)
            || IsOwnPosition(direction))
            return false;
        return true;
    }
    public bool IsDimentionBlocked(Vector3Int direction)
    {
        if (blockedDimentions == Dimentions.none) return false;
        if (blockedDimentions.HasAny(Dimentions.one) && direction.x != 0) return true;
        if (blockedDimentions.HasAny(Dimentions.two) && direction.z != 0) return true;
        if (blockedDimentions.HasAny(Dimentions.three) && direction.y != 0) return true;
        return false;
    }
    public bool IsBackwardsBlocked(Vector3Int direction, bool isWhite)       //TODO test it
    {
        if (backwardsBlocker == Axis.None) return false;

        if (isWhite)
        {
            if (backwardsBlocker.HasAny(Axis.X) && (direction.x < 0)) return true;
            if (backwardsBlocker.HasAny(Axis.Y) && (direction.y < 0)) return true;
            if (backwardsBlocker.HasAny(Axis.Z) && (direction.z < 0)) return true;
        }
        else
        {
            if (backwardsBlocker.HasAny(Axis.X) && (direction.x > 0)) return true;
            if (backwardsBlocker.HasAny(Axis.Y) && (direction.y > 0)) return true;
            if (backwardsBlocker.HasAny(Axis.Z) && (direction.z > 0)) return true;
        }
        return false;
    }
    #region -------- DIMENTIONAL LIMITS
    public bool IsWithinDimentionalLimits(Vector3Int direction) => dimentionalLimits.HasAny(DimentionalLimits(direction));      //TODO test it
    public static Dimentions DimentionalLimits(Vector3Int direction)      //TODO test it
    {
        byte rank = DimentionalRank(direction);
        return RankAsDimentions(rank);
    }
    static byte DimentionalRank(Vector3Int direction)
    {
        byte rank = 0;
        if (direction.x != 0) rank++;
        if (direction.y != 0) rank++;
        if (direction.z != 0) rank++;
        return rank;
    }
    static Dimentions RankAsDimentions(byte rank)
    {
        switch (rank)
        {
            case 1: return Dimentions.one;
            case 2: return Dimentions.two;
            case 3: return Dimentions.three;
            default: return Dimentions.none;
        }
    }
    #endregion //DIMENTIONAL LIMITS

    #region -------- DIMENTIONAL BINDINGS 
    public bool IsWithinDimentionalBinding(Vector3Int direction)      //TODO test it
    {
        //TODO 
        List<int> binds = DimentionalBinds();
        if (binds.Count == 0) return true;

        Dimentions dimentionalLimits = DimentionalLimits(direction);
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
        if (movimentLimits.Count == 0) return true;
        List<int> difVectors = direction.AbsolutesOverZero();
        return IsWithinMovimentLimits(difVectors);
    }
    public bool IsWithinMovimentLimits(List<int> direction)       //TODO test it
    {
        List<int> limitsCache = new List<int>(movimentLimits);
        List<int> directionCache = new List<int>(direction);
        //TODO otimizar essa verificação (só precisa retornar a quantidade de elementos que não deram match)
        limitsCache.RemoveMatchingElements(directionCache);
        switch (limitType)
        {
            case LimitType.atMostAll: return directionCache.Count == 0;
            case LimitType.atLeastAll: return limitsCache.Count == 0;
            case LimitType.all: return limitsCache.Count == 0 && directionCache.Count == 0;
            default: return true;
        }
    }
    #endregion //MOVIMENT LIMITS
    public static bool IsOwnPosition(Vector3Int direction) => direction == Vector3Int.zero;     //TODO test it
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

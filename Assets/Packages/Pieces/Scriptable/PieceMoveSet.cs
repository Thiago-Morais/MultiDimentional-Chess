using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using ExtensionMethods;
using System.Linq;
using UnityEngine.Animations;

[CreateAssetMenu(menuName = "Scriptable/" + nameof(PieceMoveSet))]
public class PieceMoveSet : ScriptableObject
{
    public Dimentions dimentionalLimits = Dimentions.all;
    public Dimentions dimentionalBinding = Dimentions.none;
    [Tooltip("Number of squares this piece can move on this dimention.\n" + "0 means no limit.")]
    public LimitType limitType = LimitType.atMostAll;
    public List<int> movimentLimits = new List<int>();
    public Axis axisBlocker = Axis.None;
    public Axis backwardsBlocker = Axis.None;
    public bool IsMovimentAvailable(Vector3Int dif, bool isWhite)
    {
        if (HasBlockedAxis(dif)
            || HasBlockedBackwards(dif, isWhite)
            || !IsWithinDimentionalLimits(dif)
            || !IsWithinDimentionalBinding(dif)
            || !IsWithinMovimentLimits(dif)
            || IsOwnPosition(dif))
            return false;
        return true;
    }
    public bool HasBlockedAxis(Vector3Int dif)
    {
        if (axisBlocker == Axis.None) return false;

        if (axisBlocker.HasAny(Axis.X) && (dif.x != 0)) return true;
        if (axisBlocker.HasAny(Axis.Y) && (dif.y != 0)) return true;
        if (axisBlocker.HasAny(Axis.Z) && (dif.z != 0)) return true;
        return false;
    }
    public bool HasBlockedBackwards(Vector3Int dif, bool isWhite)
    {
        if (backwardsBlocker == Axis.None) return false;

        if (isWhite)
        {
            if (backwardsBlocker.HasAny(Axis.X) && (dif.x < 0)) return true;
            if (backwardsBlocker.HasAny(Axis.Y) && (dif.y < 0)) return true;
            if (backwardsBlocker.HasAny(Axis.Z) && (dif.z < 0)) return true;
        }
        else
        {
            if (backwardsBlocker.HasAny(Axis.X) && (dif.x > 0)) return true;
            if (backwardsBlocker.HasAny(Axis.Y) && (dif.y > 0)) return true;
            if (backwardsBlocker.HasAny(Axis.Z) && (dif.z > 0)) return true;
        }
        return false;
    }
    #region -------- DIMENTIONAL LIMITS
    public bool IsWithinDimentionalLimits(Vector3Int dif) => dimentionalLimits.HasAny(DimentionalLimits(dif));
    public static Dimentions DimentionalLimits(Vector3Int dif)
    {
        byte rank = DimentionalRank(dif);
        return RankAsDimentions(rank);
    }
    static byte DimentionalRank(Vector3Int dif)
    {
        byte rank = 0;
        if (dif.x != 0) rank++;
        if (dif.y != 0) rank++;
        if (dif.z != 0) rank++;
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
    public bool IsWithinDimentionalBinding(Vector3Int dif)
    {
        //TODO 
        List<int> binds = DimentionalBinds();
        if (binds.Count == 0) return true;

        Dimentions dimentionalLimits = DimentionalLimits(dif);
        if (!dimentionalBinding.HasAny(dimentionalLimits)) return false;

        List<int> limits = dif.AsList();
        if (dimentionalLimits == Dimentions.two) return HasBindedLimits(limits, 2);
        if (dimentionalLimits == Dimentions.three) return HasBindedLimits(limits, 3);
        return true;
    }
    public List<int> DimentionalBinds() => DimentionalBinds(dimentionalBinding);
    public static List<int> DimentionalBinds(Dimentions dimentionalBinding)
    {
        List<int> bind = new List<int>();
        if (dimentionalBinding.HasAny(Dimentions.one)) bind.Add(1);
        if (dimentionalBinding.HasAny(Dimentions.two)) bind.Add(2);
        if (dimentionalBinding.HasAny(Dimentions.three)) bind.Add(3);
        return bind;
    }
    public static bool HasBindedLimits(List<int> limits, int bindingAmount)
    {
        var groupedLimits = limits.GroupBy(limit => Mathf.Abs(limit));
        var binded = groupedLimits.Where(group => group.Key != 0 && group.Count() >= bindingAmount);
        return binded.Count() > 0;
    }
    #endregion //DIMENTIONAL BINDINGS 

    #region -------- MOVIMENT LIMITS
    public bool IsWithinMovimentLimits(Vector3Int dif)
    {
        if (movimentLimits.Count == 0) return true;
        List<int> difVectors = dif.AbsolutesOverZero();
        return IsWithinMovimentLimits(difVectors);
    }
    public bool IsWithinMovimentLimits(List<int> _difVectors)
    {
        List<int> limitsCache = new List<int>(movimentLimits);
        List<int> difVectorsCahe = new List<int>(_difVectors);
        //TODO otimizar essa verificação (só precisa retornar a quantidade de elementos que não deram match)
        RemoveMatchingElements(limitsCache, difVectorsCahe);
        switch (limitType)
        {
            case LimitType.atMostAll: return difVectorsCahe.Count == 0;
            case LimitType.atLeastAll: return limitsCache.Count == 0;
            case LimitType.all: return limitsCache.Count == 0 && difVectorsCahe.Count == 0;
            default: return true;
        }
    }
    public static void RemoveMatchingElements(List<int> limits, List<int> difVectors)
    {
        int i = 0;
        while (i < limits.Count)
        {
            int limit = limits[i];

            if (difVectors.Contains(limit))
            {
                difVectors.Remove(limit);
                limits.Remove(limit);
                i = -1;
            }

            i++;
        }
    }
    #endregion //MOVIMENT LIMITS
    public static bool IsOwnPosition(Vector3Int dif) => dif == Vector3Int.zero;
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

using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using ExtensionMethods;
using System.Linq;

[CreateAssetMenu(menuName = "Scriptable/" + nameof(PieceMoveSet))]
public class PieceMoveSet : ScriptableObject
{
    public Dimentions dimentionalLimits = Dimentions.all;
    public Dimentions dimentionalBinding = Dimentions.none;
    [Tooltip("Number of squares this piece can move on this dimention.\n" + "0 means no limit.")]
    public LimitType limitType = LimitType.atMostAll;
    // public List<MinMaxCurve> movimentLimits = new List<MinMaxCurve>(new MinMaxCurve[3]);
    public List<int> movimentLimits = new List<int>();
    public bool IsMovimentAvailable(Vector3Int dif)
    {
        if (IsPiecePosition(dif)
            || !IsWithinDimentionalLimits(dif)
            || !IsWithinDimentionalBinding(dif)
            || !IsWithinMovimentLimits(dif))
            return false;
        return true;
    }
    public static bool IsPiecePosition(Vector3Int dif) => dif == Vector3Int.zero;
    #region -------- DIMENTIONAL LIMITS
    public bool IsWithinDimentionalLimits(Vector3Int dif)
    {
        Dimentions dimentionalRank = GetDimentionalRank(dif);
        return dimentionalLimits.HasAny(dimentionalRank);
    }
    public static Dimentions GetDimentionalRank(Vector3Int dif)
    {
        uint rank = 0;
        if (dif.x != 0) rank++;
        if (dif.y != 0) rank++;
        if (dif.z != 0) rank++;

        Dimentions dimentionalRank;
        switch (rank)
        {
            case 1: dimentionalRank = Dimentions.one; break;
            case 2: dimentionalRank = Dimentions.two; break;
            case 3: dimentionalRank = Dimentions.three; break;
            default: dimentionalRank = Dimentions.none; break;
        }
        return dimentionalRank;
    }
    #endregion //DIMENTIONAL LIMITS

    #region -------- DIMENTIONAL BINDINGS 
    public bool IsWithinDimentionalBinding(Vector3Int dif)
    {
        //TODO 
        /*
        verifica se o movimento tem uma quantidade expecificadas de dimenções com o mesmo numero
        */
        List<int> binds = DimentionalBindsAsList();
        if (binds.Count == 0) return true;

        List<int> dimentions = dif.AsList();

        IEnumerable<IGrouping<int, int>> enumerable = dimentions.GroupBy(c => Mathf.Abs(c));
        foreach (int bind in binds)
        {
            IEnumerable<IGrouping<int, int>> enumerable2 = enumerable.Where(binded => binded.Count() >= bind);
            if (enumerable2.Count() > 0) return true;
        }
        // IEnumerable<int> enumerable1 = enumerable.Select(grp => grp.Count(c => c >= 0));
        // List<int> list = enumerable1.ToList();

        // IEnumerable<IGrouping<int, int>> equalDistances = dimentions.GroupBy(c => Mathf.Abs(c));
        // foreach (IGrouping<int, int> equalDistance in equalDistances)
        // {
        //     int count = equalDistance.Count(c => c != 0);
        //     if (binds.Any(b => count >= b)) return true;
        // }
        return false;
    }
    List<int> DimentionalBindsAsList()
    {
        List<int> bind = new List<int>();
        if (dimentionalBinding.HasAny(Dimentions.one)) bind.Add(1);
        if (dimentionalBinding.HasAny(Dimentions.two)) bind.Add(2);
        if (dimentionalBinding.HasAny(Dimentions.three)) bind.Add(3);
        return bind;
    }
    #endregion //DIMENTIONAL BINDINGS 

    #region -------- MOVIMENT LIMITS
    public bool IsWithinMovimentLimits(Vector3Int dif)
    {
        //TODO otimizar essa verificação
        // List<int> limits = GetPositiveLimitsAsList();
        // if (limits.Count == 0) return true;
        if (movimentLimits.Count == 0) return true;

        // List<int> difVectors = new List<int> { Mathf.Abs(dif.x), Mathf.Abs(dif.y), Mathf.Abs(dif.z), };
        List<int> list = dif.AsList(i => Mathf.Abs(i));
        List<int> difVectors = list.Where(i => i > 0).ToList();

        List<int> limits = new List<int>(movimentLimits);

        RemoveMatchingElements(limits, difVectors);
        switch (limitType)
        {
            case LimitType.atMostAll:
                return difVectors.Count == 0;
            case LimitType.atLeastAll:
                return limits.Count == 0;
            case LimitType.all:
                return limits.Count == 0 && difVectors.Count == 0;
            default: return true;
        }
        // RemoveMatchingElements(limits, difVectors);

        // return limits.Count == 0;
    }
    List<int> GetPositiveLimitsAsList()
    {
        List<int> limits = new List<int>();
        foreach (MinMaxCurve limitMinMax in movimentLimits)
        {
            int limit = (int)limitMinMax.constant;
            if (limit > -1) limits.Add(limit);
        }
        return limits;
    }
    static void RemoveMatchingElements(List<int> limits, List<int> difVectors)
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

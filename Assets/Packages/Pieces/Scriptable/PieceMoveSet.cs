using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using ExtensionMethods;

[CreateAssetMenu(menuName = "Scriptable/" + nameof(PieceMoveSet))]
public class PieceMoveSet : ScriptableObject
{
    public Dimentions dimentionalLimits = Dimentions.all;
    public Dimentions dimentionalBinding = Dimentions.none;
    // public List<DimentionBind> movimentLimit = new List<DimentionBind>(new DimentionBind[3]);
    [Tooltip("Number of squares this piece can move on this dimention.\n" + "0 means no limit.")]
    /// <summary> Number of squares this piece can move on this dimention.\n" + "0 means no limit. </summary>
    public List<MinMaxCurve> movimentLimits = new List<MinMaxCurve>(new MinMaxCurve[3]);
    public bool IsMovimentAvailable(Vector3Int dif)
    {
        if (IsPiecePosition(dif)
            || !IsWithinDimentionLimits(dif)
            || !IsWithinDimentionalBinding(dif)
            || !IsWithinMovimentLimits(dif))
            return false;
        return true;
    }
    public static bool IsPiecePosition(Vector3Int dif) => dif == Vector3Int.zero;
    public bool IsWithinDimentionLimits(Vector3Int dif)
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
    public bool IsWithinDimentionalBinding(Vector3Int dif)
    {
        //TODO
        return true;
    }
    public bool IsWithinMovimentLimits(Vector3Int dif)
    {
        //TODO otimizar essa verificação
        List<int> limits = new List<int>();
        foreach (MinMaxCurve limitMinMax in movimentLimits)
        {
            int limit = (int)limitMinMax.constant;
            if (limit > 0) limits.Add(limit);
        }
        if (limits.Count == 0) return true;

        List<int> difVectors = new List<int>();
        if (dif.x > 0) difVectors.Add(dif.x);
        if (dif.y > 0) difVectors.Add(dif.y);
        if (dif.z > 0) difVectors.Add(dif.z);

        RemoveMatchingElements(limits, difVectors);

        return limits.Count == 0 && difVectors.Count == 0;
    }
    private static void RemoveMatchingElements(List<int> limits, List<int> difVectors)
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
}
[Serializable]
public class DimentionBind
{
    [Tooltip("Number of squares this piece can move on this dimention.\n" + "0 means no limit.")]
    public MinMaxCurve distanceLimit;
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
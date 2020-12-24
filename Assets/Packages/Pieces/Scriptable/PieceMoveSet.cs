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
    public List<MinMaxCurve> movimentLimit = new List<MinMaxCurve>(new MinMaxCurve[3]);
    public bool IsMoveAvailable(Vector3Int dif)
    {
        if (dif == Vector3Int.zero) return false;
        if (!IsWithinDimentionLimits(dif)) return false;
        return true;
    }
    bool IsWithinDimentionLimits(Vector3Int dif)
    {
        Dimentions dimentionalRank = GetDimentionalRank(dif);
        return dimentionalLimits.HasAny(dimentionalRank);
    }
    Dimentions GetDimentionalRank(Vector3Int dif)
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
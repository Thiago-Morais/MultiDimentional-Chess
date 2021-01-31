using UnityEngine;

namespace ExtensionMethods
{
    public static class OnBoardExtensions
    {
        public static Vector3Int VectorDifference(this IOnBoard a, IOnBoard b) => b.BoardCoord - a.BoardCoord;
    }
}
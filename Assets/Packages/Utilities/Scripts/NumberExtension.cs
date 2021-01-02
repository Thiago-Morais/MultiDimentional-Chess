using System;

namespace ExtensionMethods
{
    public static class NumberExtension
    {
        public static bool IsEven(this int value) => value % 2 == 0;
        public static T RankAsFlags<T>(this int rank) where T : Enum
        {
            if (rank != 0) rank = (int)Math.Pow(2, rank - 1);
            return (T)(object)rank;
        }
    }
}
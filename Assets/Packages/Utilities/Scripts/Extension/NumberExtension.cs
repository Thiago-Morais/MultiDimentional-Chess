using System;

namespace ExtensionMethods
{
    public static class NumberExtension
    {
        public static bool IsEven(this int value) => value % 2 == 0;
        public static T RankAs<T>(this int rank) where T : Enum
        {
            if (rank != 0) rank = (int)Math.Pow(2, rank - 1);
            return (T)(object)rank;
        }
        public static float InverseLerpUnclamped(this float value, float minValue, float maxValue)
        {
            if (minValue != maxValue)
                return (value - minValue) / (maxValue - minValue);
            else
                return 0.0f;
        }
    }
}
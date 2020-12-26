using System;

namespace ExtensionMethods
{
    public static class MultidimentionalArray
    {
        public static void ActionOnBoard<T>(this T[] array, Action<T[], int> Action)
        {
            for (int i = 0; i < array.GetLength(0); i++)
                Action(array, i);
        }
        public static void ActionOnBoard<T>(this T[,] array, Action<T[,], int, int> Action)
        {
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    Action(array, i, j);
        }
        public static T[,,] ForEachDoAction<T>(this T[,,] array, Action<int, int, int> Action)
        {
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    for (int k = 0; k < array.GetLength(2); k++)
                        Action(i, j, k);
            return array;
        }
        public static bool OutOfBounds<T>(this T[] array, int i)
            => i > array.GetUpperBound(0);
        public static bool OutOfBounds<T>(this T[,] array, int i, int j)
            => i > array.GetUpperBound(0) || j > array.GetUpperBound(1);
        public static bool OutOfBounds<T>(this T[,,] array, int i, int j, int k)
            => i > array.GetUpperBound(0) || j > array.GetUpperBound(1) || k > array.GetUpperBound(2);
        public static T RemoveAt<T>(this T[,,] array, int i, int j, int k)
        {
            T square = array[i, j, k];
            array[i, j, k] = default(T);
            return square;
        }
    }
}
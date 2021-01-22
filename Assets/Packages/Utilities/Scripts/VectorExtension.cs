using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExtensionMethods
{
    public static class VectorExtension
    {
        public static List<float> AsList(this Vector2 vector) => new List<float> { vector.x, vector.y };        //TODO test it
        public static List<float> AsList(this Vector3 vector) => new List<float> { vector.x, vector.y, vector.z };      //TODO test it
        public static List<int> AsList(this Vector2Int vector) => new List<int> { vector.x, vector.y, };        //TODO test it
        public static List<int> AsList(this Vector3Int vector) => new List<int> { vector.x, vector.y, vector.z };       //TODO test it
        public static List<int> AsList(this Vector3Int vector, Func<int, int> func) => new List<int> { func(vector.x), func(vector.y), func(vector.z) };        //TODO test it
        public static List<int> AbsolutesOverZero(this Vector3Int vector)       //TODO test it
        {
            List<int> difLimits = vector.AsList(i => Mathf.Abs(i));
            return difLimits.Where(i => i > 0).ToList();
        }
        public static Vector3 Scaled(this Vector3 vector, Vector3 scale)        //TODO test it
        {
            Vector3 aux = new Vector3(vector.x, vector.y, vector.z);
            aux.Scale(scale);
            return aux;
        }
        public static Vector3Int AsInt(this Vector3 vector) => Vector3Int.RoundToInt(vector);
        public static int Rank(this Vector3Int vector)
        {
            int rank = 0;
            if (vector.x != 0) rank++;
            if (vector.y != 0) rank++;
            if (vector.z != 0) rank++;
            return rank;
        }
        public static bool IsBinded(this Vector3Int vector) => vector.x == vector.y && vector.y == vector.z;
        public static Binding IsBindedIgnoringZero(this Vector3Int vector)
        {
            Binding binding = new Binding(isBinded: true);

            List<int> bindedValues = new List<int>() { Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z) }.RemoveAll(0);
            if (bindedValues.Count == 0 || bindedValues.Count == 1)
                binding.isBinded = true;
            else
                for (int i = 0; i < bindedValues.Count - 1; i++)
                    if (bindedValues[i] != bindedValues[i + 1])
                    {
                        binding.isBinded = false;
                        break;
                    }
            return binding;
        }
        public static bool IsBindedIgnoringZero(this Vector3Int vector, ref int binded)
        {
            List<int> bindedValues = new List<int>() { Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z) }.RemoveAll(0);
            if (bindedValues.Count == 0) return true;

            int cachedBind = binded = bindedValues[0];
            if (bindedValues.Count == 1) return true;

            for (int i = 1; i < bindedValues.Count; i++)
            {
                int bind = bindedValues[i];
                if (bind != cachedBind) return false;

                cachedBind = bind;
            }
            return true;
        }
    }
}
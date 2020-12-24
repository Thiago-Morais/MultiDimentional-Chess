using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExtensionMethods
{
    public static class VectorExtension
    {
        public static List<float> AsList(this Vector2 vector) => new List<float> { vector.x, vector.y };
        public static List<float> AsList(this Vector3 vector) => new List<float> { vector.x, vector.y, vector.z };
        public static List<int> AsList(this Vector2Int vector) => new List<int> { vector.x, vector.y, };
        public static List<int> AsList(this Vector3Int vector) => new List<int> { vector.x, vector.y, vector.z };
        public static List<int> AsList(this Vector3Int vector, Func<int, int> func) => new List<int> { func(vector.x), func(vector.y), func(vector.z) };
    }
}
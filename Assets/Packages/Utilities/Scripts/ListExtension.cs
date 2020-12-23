using System.Collections.Generic;

namespace ExtensionMethods
{
    public static class ListExtension
    {
        public static bool IsEmpty<T>(this List<T> list) => list == null || list.Count == 0;
    }
}
using System.Collections.Generic;

namespace ExtensionMethods
{
    public static class ListExtension
    {
        public static bool IsEmpty<T>(this List<T> list) => list == null || list.Count <= 0;
        public static T Pop<T>(this List<T> list)
        {
            if (list.IsEmpty()) return default(T);

            T item = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return item;
        }
        public static void Push<T>(this List<T> list, T item) => list.Add(item);
        public static T Peek<T>(this List<T> list)
        {
            if (list.IsEmpty()) throw new System.InvalidOperationException("List is empty");

            return list[list.Count - 1];
        }
        public static void RemoveMatchingElements<T>(this List<T> listA, List<T> listB)
        {
            int i = 0;
            while (i < listA.Count)
            {
                T item = listA[i];

                if (listB.Contains(item))
                {
                    listB.Remove(item);
                    listA.Remove(item);
                    i = 0;
                }
                else i++;
            }
        }
        public static List<int> RemoveAll(this List<int> values, int value)
        {
            for (int i = 0; i < values.Count; i++)
                if (values[i] == value) values.Remove(values[i--]);
            return values;
        }
    }
}
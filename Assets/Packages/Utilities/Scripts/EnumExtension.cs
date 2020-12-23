using System;

namespace ExtensionMethods
{
    public static class EnumExtension
    {
        public static bool HasAll<T, W>(this T source, W op) where T : Enum where W : Enum
        {
            int a = (int)(object)source;
            int b = (int)(object)op;
            return (a & b) == b;
        }
        public static bool HasAny<T, W>(this T source, W op) where T : Enum where W : Enum
        {
            int a = (int)(object)source;
            int b = (int)(object)op;
            return (a & b) != 0;
        }
        public static T Include<T, W>(this T source, W op) where T : Enum where W : Enum
        {
            int a = (int)(object)source;
            int b = (int)(object)op;
            return (T)(object)(a | b);
        }
        public static T Exclude<T, W>(this T source, W op) where T : Enum where W : Enum
        {
            int a = (int)(object)source;
            int b = (int)(object)op;
            return (T)(object)(a & ~b);
        }
    }
}
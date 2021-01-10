using UnityEngine;

namespace ExtensionMethods
{
    public static class MaterialExtension
    {
        public static void SetKeyword(this Material material, string name, bool value)
        {
            if (value)
                material.EnableKeyword(name);
            else
                material.DisableKeyword(name);
        }
    }
}
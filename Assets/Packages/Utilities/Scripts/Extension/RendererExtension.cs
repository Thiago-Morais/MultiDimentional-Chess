using UnityEngine;

namespace ExtensionMethods
{
    public static class RendererExtensions
    {
        public static void SetMaterials(this Renderer[] renderers, Material material)
        {
            foreach (Renderer renderer in renderers)
                renderer.material = material;
        }
    }
}
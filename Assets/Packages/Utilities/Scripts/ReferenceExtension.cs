using UnityEngine;

namespace ExtensionMethods
{
    public static class ReferenceExtension
    {
        public static void ClampNormalized(this Vector3 axis) => axis = axis.magnitude > axis.normalized.magnitude ? axis.normalized : axis;
        public static T Initialized<T, W>(this T component, W classReference) where T : Object where W : Component
        {
            if (!component) component = classReference.GetComponentInChildren<T>();
            return component;
        }
    }
    public static class RendererExtensions
    {
        public static void SetMaterials(this Renderer[] renderers, Material material)
        {
            foreach (Renderer renderer in renderers)
                renderer.material = material;
        }
    }
}
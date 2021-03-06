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
        public static T InstantiateInitialized<T>(string name = "GameObject") where T : MonoBehaviour, IInitializable
        {
            GameObject gameObject = new GameObject(name);
            T t = gameObject.AddComponent<T>();
            IInitializable initializable = t.Initialized();
            return initializable as T;
        }
    }
}
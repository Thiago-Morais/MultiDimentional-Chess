using UnityEngine;

public interface IPoolable : IInitializable
{
    // IPoolable Deactivated();
    // IPoolable Activated();
    // IPoolable InstantiatePoolable();
    Component Deactivated();
    Component Activated();
    Component InstantiatePoolable();
    Transform transform { get; }
}
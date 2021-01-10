using UnityEngine;

public interface IPoolable : IInitializable
{
    Component Deactivated();
    Component Activated();
    Component InstantiatePoolable();
    Transform transform { get; }
}
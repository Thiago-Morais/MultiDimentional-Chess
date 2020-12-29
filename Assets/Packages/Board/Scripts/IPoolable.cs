using UnityEngine;

public interface IPoolable
{
    IPoolable Deactivated();
    IPoolable Activated();
    IPoolable InstantiatePoolable();
}
using UnityEngine;

public interface IPoolable
{
    IPoolable Deactivated();
    IPoolable Activated();
    // IPoolable Instantiate();
    IPoolable Instantiate(Transform poolParent);
}
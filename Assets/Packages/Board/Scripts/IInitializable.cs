using UnityEngine;

public interface IInitializable
{
    IInitializable Initialized(Transform parent = null);
}
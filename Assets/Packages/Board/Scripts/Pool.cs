using System;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

[Serializable]
public class Pool<T> where T : IPoolable
{
    public GameObject prefabSample;
    public T sample;
    public List<T> objectPool = new List<T>();
    public Transform poolParent;
    public void SetSample() => sample = prefabSample.GetComponentInChildren<T>();
    public T GetFromPool(T poolable) =>
        (objectPool.IsEmpty()) ?
            (T)poolable.Instantiate(poolParent) :
            (T)objectPool.Pop().Activated();
    public void PushToPool(T obj)
    {
        if (obj == null) return;
        objectPool.Add((T)obj.Deactivated());
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

[Serializable]
public class Pool<T> where T : Component, IPoolable
{
    public T sample;
    public List<T> objectPool = new List<T>();
    public Transform poolParent;
    public Pool() { }
    public Pool(List<T> poolables) => objectPool = poolables;
    public Pool(T sample, List<T> objectPool, Transform poolParent)
    {
        this.sample = sample;
        this.objectPool = objectPool;
        this.poolParent = poolParent;
    }
    public void Awake() => Initialized();
    public Pool<T> Initialized()
    {
        if (!sample) sample = new GameObject().AddComponent<T>();
        if (!poolParent) poolParent = new GameObject().transform;
        return this;
    }
    public T GetFromPool()
    {
        T poolableInstance;
        if (objectPool.IsEmpty())
            poolableInstance = (T)UnityEngine.Object.Instantiate<T>(sample);
        else
            poolableInstance = (T)objectPool.Pop()?.Activated();
        return poolableInstance;
    }
    public T GetFromPoolGrouped()
    {
        T poolableInstance = GetFromPool();
        poolableInstance.gameObject.transform.SetParent(poolParent);
        return poolableInstance;
    }
    public void PushToPool(T obj)
    {
        if (obj == null) return;
        objectPool.Add((T)obj.Deactivated());
    }
}

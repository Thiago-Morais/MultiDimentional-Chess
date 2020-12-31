using System;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

[Serializable]
// public class Pool<T> where T : Component, IPoolable
public class Pool : MonoBehaviour
{
    #region -------- FIELDS
    public GameObject prefab;
    public IPoolable sample;
    public List<Component> objectPool = new List<Component>();
    #endregion //FIELDS

    #region -------- PROPERTIES
    #endregion //PROPERTIES

    #region -------- METHODS
    public Pool InitializedAs<T>() where T : Component, IPoolable
    {
        if (prefab) SetSampleWithPrefab<T>();
        else if (sample == null)
        {
            GameObject samplePrefab = new GameObject(typeof(T).Name);
            samplePrefab.transform.SetParent(transform);
            sample = samplePrefab.AddComponent<T>().Initialized() as IPoolable;
        }
        if (objectPool == null) objectPool = new List<Component>();
        return this;
    }
    public T GetFromPool<T>() where T : Component, IPoolable
    {
        if (sample == null) InitializedAs<T>();
        T poolableInstance;
        if (objectPool.IsEmpty())
            poolableInstance = sample.InstantiatePoolable() as T;
        else
            poolableInstance = (objectPool.Pop() as IPoolable).Activated() as T;
        return poolableInstance;
    }
    public T GetFromPoolGrouped<T>() where T : Component, IPoolable
    {
        T poolableInstance = GetFromPool<T>();
        poolableInstance.transform.SetParent(transform);
        return poolableInstance;
    }
    public void PushToPool<T>(T obj) where T : Component, IPoolable
    {
        if (obj == null) return;
        objectPool.Add((T)obj.Deactivated());
    }
    [ContextMenu(nameof(SetSampleWithPrefab))]
    public void SetSampleWithPrefab<T>() => sample = prefab.GetComponent<T>() as IPoolable;
    #endregion //METHODS
}
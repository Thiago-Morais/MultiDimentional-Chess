using System;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public partial class CreateBoard
{
    [Serializable]
    public class Pool
    {
        public Stack<GameObject> objectPool;
        public Transform poolParent;
        public GameObject GetFromPool(GameObject gameObject)
        {
            GameObject poolObj;
            if (objectPool != null && objectPool.Count > 0)
            {
                poolObj = objectPool.Pop();
                poolObj.SetActive(true);
            }
            else
                poolObj = Instantiate(gameObject, poolParent);
            return poolObj;
        }
        public bool PushToPool(GameObject obj)
        {
            if (!obj) return false;
            if (objectPool == null) objectPool = new Stack<GameObject>();

            objectPool.Push(obj);
            obj.SetActive(false);
            return true;
        }
    }
    [Serializable]
    public class Pool<T> where T : IPoolable
    {
        public GameObject prefabSample;
        public T sample;
        // public Stack<T> objectPool = new Stack<T>();
        public List<T> objectPool = new List<T>();
        public Transform poolParent;
        public void SetSample() => sample = prefabSample.GetComponentInChildren<T>();
        public T GetFromPool(T poolable) =>
            (objectPool.IsEmpty()) ?
                (T)poolable.Instantiate(poolParent) :
                // (T)objectPool.Pop().Activated();
                (T)objectPool.Pop().Activated();
        public void PushToPool(T obj)
        {
            if (obj == null) return;
            // objectPool.Push((T)obj.Deactivated());
            objectPool.Add((T)obj.Deactivated());
        }
    }
}

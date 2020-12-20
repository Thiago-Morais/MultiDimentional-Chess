using System;
using System.Collections.Generic;
using UnityEngine;

public partial class CreateBoard
{
    [Serializable]
    public class Pool
    {
        public Stack<GameObject> objectPool;
        public Transform poolParent;
        // GameObject GetFromPool(SO_BoardPiece squareData)
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
}

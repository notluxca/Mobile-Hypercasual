using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private PoolableObject objectPrefab;

    private readonly Queue<PoolableObject> pool = new Queue<PoolableObject>();

    public GameObject GetObject()
    {
        PoolableObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(objectPrefab, transform);
        }

        obj.gameObject.SetActive(true);
        obj.pool = this;
        obj.InitializePoolObject();
        return obj.gameObject;
    }

    public void ReturnObject(PoolableObject obj)
    {
        if (obj == null) return;

        obj.DeactivatePoolObject();
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}

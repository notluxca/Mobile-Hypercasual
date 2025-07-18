using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public ObjectPool pool; // assigned by the pool when instantiated

    // Provides a customizable interface for objects that require different initialization
    public virtual void InitializePoolObject() { }

    // Called when the object is deactivated before returning to the pool
    public virtual void DeactivatePoolObject() { }

    public virtual void ReturnToPool()
    {
        pool.ReturnObject(this);
    }
}

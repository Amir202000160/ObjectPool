using UnityEngine;

public class PooledObject : MonoBehaviour
 {
    private ObjectPool pool;
    public ObjectPool Pool { get => pool; set => pool = value; }
    public void Release()
    {
        pool.ReturnToPool(this);
    }

    void Update()
    {
        if (transform.position.z > 20.0f)
        {
            Release();
        }
    }
} 

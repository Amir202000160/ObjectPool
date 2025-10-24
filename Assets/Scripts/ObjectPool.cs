using System.Collections.Generic;
using UnityEngine;

 public class ObjectPool : MonoBehaviour
 {
    [SerializeField] private int initPoolSize;
    [SerializeField] private PooledObject objectToPool;
    // Store the pooled objects in a collection
    private Stack<PooledObject> stack;
    private void Start()
    {
        SetupPool();
    }
    // Creates the pool (invoke when the lag is not noticeable)
    private void SetupPool()
    {
        stack = new Stack<PooledObject>();
        PooledObject instance = null;
        for (int i = 0; i < initPoolSize; i++)
        {
            instance = Instantiate(objectToPool, transform.position, transform.rotation);
            instance.Pool = this;
            instance.gameObject.SetActive(false);
            stack.Push(instance);
        }
    }
    // returns the first active GameObject from the pool
    public PooledObject GetPooledObject()
    {
        // if the pool is not large enough, instantiate a new  PooledObjects
        if (stack.Count == 0)
        {
            PooledObject newInstance = Instantiate(objectToPool, transform.position, transform.rotation);
            newInstance.Pool = this;
            return newInstance;
        }
        // otherwise, just grab the next one from the list
        PooledObject nextInstance = stack.Pop();
        nextInstance.gameObject.SetActive(true);
        return nextInstance;
    }

    public void ReturnToPool(PooledObject pooledObject)
    {
        stack.Push(pooledObject);
        pooledObject.gameObject.SetActive(false);
        pooledObject.transform.position = transform.position;
    }


 }

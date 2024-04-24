using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour, IPoolableObject
{
    private readonly Transform objectPoolTransform;
    private readonly T objectPrefab;
    private readonly int PreCreateNumber;

    private readonly Queue<T> objectQueue = new();

    public ObjectPool(T objectPrefab, int preCreateNumber = 0, Transform parentObjectTransform = null)
    {
        this.objectPrefab = objectPrefab;
        this.PreCreateNumber = preCreateNumber;
        this.objectPoolTransform = parentObjectTransform;

        for (int i = 0; i < PreCreateNumber; i++)
        {
            objectQueue.Enqueue(CreateObject());
        }
    }

    public T GetObject(bool setObjectActive = true)
    {
        T obj;

        if (objectQueue.Count > 0)
            obj = objectQueue.Dequeue();
        else
            obj = CreateObject();

        obj.gameObject.SetActive(setObjectActive);

        return obj;
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(objectPoolTransform);

        objectQueue.Enqueue(obj);
    }

    private T CreateObject()
    {
        T obj = Object.Instantiate(objectPrefab, Vector3.zero, objectPrefab.transform.rotation, objectPoolTransform);
        obj.gameObject.SetActive(false);
        obj.SetPool(this);

        return obj;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling<T> : MonoBehaviour
{
    private GameObject example;
    private Transform storage;

    private Queue<T> poolOfObjects;

    public ObjectPooling(int Index, GameObject Example, Transform Storage)
    {
        example = Example;
        storage = Storage;

        poolOfObjects = new Queue<T>();


        for (int i = 0; i < Index; i++)
        {
            GameObject _object = Instantiate(Example, Storage);
            _object.SetActive(false);
            poolOfObjects.Enqueue(_object.GetComponent<T>());
        }
    }

    public ObjectPooling(int Index, GameObject Example)
    {
        example = Example;
        storage = null;

        poolOfObjects = new Queue<T>();


        for (int i = 0; i < Index; i++)
        {
            GameObject _object = Instantiate(Example);
            _object.SetActive(false);
            poolOfObjects.Enqueue(_object.GetComponent<T>());
        }
    }

    public T GetPrefab()
    {
        if (poolOfObjects.Count > 0) return poolOfObjects.Dequeue();

        print("instantiated new object of type: queue is full");
        GameObject _object = default;
        if (storage == null)
        {
            _object = Instantiate(example);
        }
        else
        {
            _object = Instantiate(example, storage);
        }

        _object.SetActive(true);

        return _object.GetComponent<T>();
    }

    public void ReturnObject(T _object)
    {   
        poolOfObjects.Enqueue(_object);
    }
}

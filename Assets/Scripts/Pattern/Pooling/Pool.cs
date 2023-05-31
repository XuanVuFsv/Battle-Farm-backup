using System.Collections.Generic;
using UnityEngine;

public interface IFactory<T>
{
    T Create();
}

public class Factory<T> : IFactory<T> where T : new()
{
    public T Create()
    {
        return new T();
    }
}

public class PrefabFactory<T> : IFactory<T> where T : MonoBehaviour
{
    readonly GameObject prefab;
    readonly string name;
    readonly Transform parent = null;
    int index = 0;

    public PrefabFactory(GameObject prefab) : this(prefab, prefab.name) { }

    public PrefabFactory(GameObject prefab, string name)
    {
        this.prefab = prefab;
        this.name = name;
    }

    public PrefabFactory(GameObject prefab, Transform parent) : this(prefab, prefab.name, parent) { }
    public PrefabFactory(GameObject prefab, string name, Transform parent)
    {
        this.prefab = prefab;
        this.name = name;
        this.parent = parent;
    }

    public T Create()
    {
        GameObject tempGameObject = GameObject.Instantiate(prefab, parent);
        tempGameObject.name = name + index.ToString();
        T objectOfType = tempGameObject.GetComponent<T>();
        tempGameObject.SetActive(true);
        index++;
        return objectOfType;
    }
}

public interface IPool
{
    void Release();
    void Dispose();
    void Reset();
}

public interface IPool<T> : IPool
{
    T Get();
}

public class Pool<T> : IPool<T> where T : IPool
{
    public Stack<T> pooledObjects = new Stack<T>();
    public Queue<T> alreadyUsedObjects = new Queue<T>();
    IFactory<T> factory;

    public int poolSize, maxSize;

    public Pool(IFactory<T> factory) : this(factory, 5) { }

    public Pool(IFactory<T> factory, int _poolSize)
    {
        poolSize = _poolSize;
        maxSize = poolSize + 5;
        this.factory = factory;

        for (int i = 0; i < poolSize; i++)
        {
            Create();
        }
    }

    public T Create()
    {
        T member = factory.Create();
        pooledObjects.Push(member);
        return member;
    }

    public T Get()
    {
        if (alreadyUsedObjects.Count >= poolSize)
        {
            if (alreadyUsedObjects.Count < maxSize)
            {
                T newpooledObjects = Create();
                alreadyUsedObjects.Enqueue(newpooledObjects);
                return newpooledObjects;
            }
            else return default(T);
        }
        else
        {
            T usedObject = pooledObjects.Pop();
            alreadyUsedObjects.Enqueue(usedObject);
            return usedObject;
        }
    }

    public void Release()
    {
        T lastObject = alreadyUsedObjects.Dequeue();
        pooledObjects.Push(lastObject);
    }

    public void Reset()
    {
        
    }

    public void Dispose()
    {
        pooledObjects = null;
        alreadyUsedObjects = null;
        factory = null;
    }
}

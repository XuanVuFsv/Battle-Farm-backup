using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public class ObjectInPoolInitCount
//{
//    public GameObject prefab;
//    public int count;
//}

public class PoolSetup : GameObserver, IPoolSetup
{
    [Tooltip("Check this to mark pool has multiple object. Otherwise pool has muliple object")]
    public bool isSameObject;
    [Tooltip("This prefab will be used if isSameObject true")]
    public GameObject prefab;
    //[Tooltip("This list will be used if isSameObject false")]
    //public List<ObjectInPoolInitCount> multipleDifferentObjectList = new List<ObjectInPoolInitCount>();

    public string poolManagerName;
    [SerializeField] GameEvent gameEvent;

    public Pool<ObjectInPool> pool;
    public ObjectInPool currentObject;
    [Tooltip("When using multipleDifferentObjectList, init pool size is predetermined which is number of all object instantiated base on ObjectInPoolInitCount at init. Max pool size is flexible")]
    [SerializeField] int initPoolSize, maxPoolSize;

   public IPool InitPool(string poolManagerName, int initPoolSize, int maxPoolSize, GameObject prefab, GameEvent gameEvent)
    {
        this.prefab = prefab;
        this.poolManagerName = poolManagerName;
        this.gameEvent = gameEvent;
        this.initPoolSize = initPoolSize;
        this.maxPoolSize = maxPoolSize;
        pool = new Pool<ObjectInPool>(new PrefabFactory<ObjectInPool>(prefab, transform), initPoolSize);
        return pool;
    }

    //public IPool InitPool(string poolManagerName, int maxPoolSize, List<ObjectInPoolInitCount> list, GameEvent gameEvent)
    //{
    //    this.multipleDifferentObjectList = list;
    //    this.poolManagerName = poolManagerName;
    //    this.gameEvent = gameEvent;

    //    int count = 0;
    //    foreach (ObjectInPoolInitCount infor in list)
    //    {
    //        count += infor.count;
    //    }
    //    this.initPoolSize = count;
    //    this.maxPoolSize = maxPoolSize;
    //    pool = new Pool<ObjectInPool>(new PrefabFactory<ObjectInPool>(prefab, transform), initPoolSize);
    //    return pool;
    //}

    public IPool InitPool()
    {
        pool = new Pool<ObjectInPool>(new PrefabFactory<ObjectInPool>(prefab, transform), initPoolSize);
        return pool;
    }

    public int GetMaxPoolSize() { return maxPoolSize; }

    public int GetPoolSize() { return pool.poolSize; }

    public string GetName() { return poolManagerName; }

    public void Get()
    {
        currentObject = pool.Get();
    }

    public void Release()
    {
        pool.Release();
    }

    public void Reset()
    {

    }

    public void Dispose()
    {
        foreach (Transform poolObject in transform)
        {
            poolObject.GetComponent<ObjectInPool>().Dispose();
            //poolObject.GetComponent<BulletHoleBehaviour>().Dispose();
        }
        pool.Dispose();
    }

    public override void Execute(IGameEvent gEvent, RaycastHit hit)
    {
        //if (!currentObject)
        //{
        //    RemoveGameEvent();
        //    return;
        //}
        currentObject?.OnUsed(hit); // fix null in memory
    }


    public override void Execute(IGameEvent gEvent, Vector3 point, Vector3 normal)
    {
        //if (!currentObject)
        //{
        //    RemoveGameEvent();
        //    return;
        //}
        //Debug.Log(this);
        //Debug.Log(gameObject.transform.parent);
        currentObject?.OnUsed(point, normal); // fix null in memory
    }

    public void AddGameEvent()
    {
        AddGameEventToObserver(gameEvent);
    }

    public void RemoveGameEvent()
    {
        RemoveGameEventFromObserver(gameEvent);
    }

    void OnDestroy()
    {
        RemoveGameEvent();
    }

    void OnDisable()
    {
        RemoveGameEvent();
    }
}

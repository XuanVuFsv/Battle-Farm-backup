using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolSetup : GameObserver, IPoolSetup
{
    public GameObject prefab;
    public string poolManagerName;
    [SerializeField] GameEvent gameEvent;

    public Pool<ObjectInPool> bulletHolePool;
    public ObjectInPool currentObject;
    [SerializeField] int initPoolSize, maxPoolSize;

   public IPool InitPool(string poolManagerName, int initPoolSize, int maxPoolSize, GameObject prefab, GameEvent gameEvent)
    {
        this.prefab = prefab;
        this.poolManagerName = poolManagerName;
        this.gameEvent = gameEvent;
        this.initPoolSize = initPoolSize;
        this.maxPoolSize = maxPoolSize;
        bulletHolePool = new Pool<ObjectInPool>(new PrefabFactory<ObjectInPool>(prefab, transform), initPoolSize);
        return bulletHolePool;
    }

    public IPool InitPool()
    {
        bulletHolePool = new Pool<ObjectInPool>(new PrefabFactory<ObjectInPool>(prefab, transform), initPoolSize);
        return bulletHolePool;
    }

    public int GetMaxPoolSize() { return maxPoolSize; }

    public int GetPoolSize() { return bulletHolePool.poolSize; }

    public string GetName() { return poolManagerName; }

    public void Get()
    {
        currentObject = bulletHolePool.Get();
    }

    public void Release()
    {
        bulletHolePool.Release();
    }

    public void Reset()
    {

    }

    public void Dispose()
    {
        foreach (Transform poolObject in transform)
        {
            poolObject.GetComponent<BulletHoleBehaviour>().Dispose();
        }
        bulletHolePool.Dispose();
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

using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    private static PoolingManager instance;

    public Dictionary<string, IPoolSetup> pooledList = new Dictionary<string, IPoolSetup>();

    public string currentPool;

    void MakeInstance()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else instance = this;
    }

    public static PoolingManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        MakeInstance();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform manager in transform)
        {
            IPoolSetup _manager = manager.GetComponent(typeof(IPoolSetup)) as IPoolSetup;
            try
            {
                _manager.InitPool();
                pooledList.Add(_manager.GetName(), _manager);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }
    }

    public void Get(string objectPooledManagerName)
    {
        pooledList[objectPooledManagerName].Get();;
    }

    public void Release(string objectPooledManagerName)
    {
        pooledList[objectPooledManagerName].Release();
    }

    public void AddPoolManager(string poolManagerName, int initPoolSize, int maxPoolSize, GameObject prefab, GameEvent gameEvent)
    {
        GameObject newPoolManager = new GameObject(poolManagerName);
        newPoolManager.transform.parent = transform;
        newPoolManager.AddComponent<PoolSetup>();
        
        pooledList.Add(poolManagerName, newPoolManager.GetComponent<PoolSetup>().InitPool(poolManagerName, initPoolSize, maxPoolSize, prefab, gameEvent) as IPoolSetup);
    }

    public void RemovePoolManagerByName(string poolManagerName)
    {
        pooledList[poolManagerName].Dispose();
        pooledList.Remove(poolManagerName);
    }

    public void ResetPoolingManager()
    {

    }

    public void AddGameEvent(string poolName)
    {
        if (!IsContainsPool(poolName))
        {
            MyDebug.Instance.Log("Pool not exist");
            currentPool = "";
            return;
        }

        pooledList[poolName].AddGameEvent();
        currentPool = poolName;
    }

    public void RemoveGameEvent(string poolName)
    {
        if (!IsContainsPool(poolName))
        {
            MyDebug.Instance.Log("Pool not exist");
            currentPool = "";
            return;
        }
        pooledList[poolName].RemoveGameEvent();
        currentPool = "";
    }

    public bool IsContainsPool(string poolName)
    {
        return pooledList.ContainsKey(poolName);
    }
}

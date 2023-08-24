using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public abstract class ObjectInPool : MonoBehaviour, IPool
{
    public float lifeTime;

    public virtual void OnUsed(RaycastHit hit) { }

    public virtual void OnUsed(Vector3 point, Vector3 normal) { }

    public void Release()
    {
        Reset();
        PoolingManager.Instance.Release(transform.parent.name);
        gameObject.SetActive(false);
    }

    public void Dispose()
    {
        Destroy(gameObject);
    }

    public void Reset()
    {

    }
}
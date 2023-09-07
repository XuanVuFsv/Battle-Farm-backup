using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoleBehaviour : ObjectInPool
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public override void OnUsed(RaycastHit hit)
    {
        gameObject.SetActive(true);

        transform.position = hit.point;
        transform.forward = hit.normal;
        transform.rotation = Quaternion.LookRotation(hit.normal);
        Invoke(nameof(Release), lifeTime);
    }

    public override void OnUsed(Vector3 point, Vector3 normal)
    {
        gameObject.SetActive(true);

        transform.position = point;
        transform.forward = normal;
        transform.rotation = Quaternion.LookRotation(normal);
        Invoke(nameof(Release), lifeTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutBehaviour : MonoBehaviour
{
    [SerializeField]
    Rigidbody rigid;
    [SerializeField]
    ParticleSystem explosion;
    public GameObject stunObject;

    public void AddForeToBullet(int force, Vector3 direction)
    {
        rigid.AddForce(direction.normalized * force);
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(3);
        if (transform)
        {
            Explode();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy") Explode();
    }

    void HandleEffect()
    {
        explosion.Emit(1);

        Destroy(stunObject, 0.1f);
        explosion.transform.parent = null;

        stunObject.transform.parent = null;
    }

    void Explode()
    {
        HandleEffect();
        Destroy(gameObject);
    }
}

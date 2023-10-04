using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform obj;
    public Transform looked;
    float timeElapsed;
    public float lerpDuration = 3;

    private void Start()
    {

    }

    //private void LateUpdate()
    //{
    //    transform.LookAt(obj);
    //}

    private void FixedUpdate()
    {
        //if (timeElapsed < lerpDuration)
        //{
        //    transform.position = Vector3.Lerp(transform.position, obj.position, timeElapsed / lerpDuration);

        //    timeElapsed += Time.deltaTime;
        //}
        //transform.position = Vector3.MoveTowards(transform.position, obj.transform.position, 1);

        //transform.LookAt(looked);
    }

    //private void Update()
    //{
    //    transform.LookAt(obj);
    //}
}

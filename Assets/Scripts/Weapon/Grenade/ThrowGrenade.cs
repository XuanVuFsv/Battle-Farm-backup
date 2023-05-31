using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGrenade : MonoBehaviour
{
    [SerializeField]
    private GameObject grenadeObject;
    [SerializeField]
    private Transform muzzle;

    [SerializeField, Min(1)]
    private int grenadeObjectMass = 1;

    [SerializeField, Min(1)]
    private float throwForce = 30;

    [SerializeField]
    private float throwDelay = 1;
    private bool isWaiting = false;

    [SerializeField]
    TrajectoryLine trajectoryLine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if ()
    }

    public void Throw()
    {
        if (!isWaiting)
        {
            GameObject grenade = Instantiate(grenadeObject);
            grenade.transform.position = muzzle.position;
            grenade.GetComponent<GrenadeBehaviour>().AddForce();
            isWaiting = true;
        }
    }
}

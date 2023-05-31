using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField]
    public bool inAim = false;
    [SerializeField]
    GameEvent aimEvent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //public void PlayAimAnimation(CameraShake cameraShake, string weaponName)
    //{
    //    inAim = !inAim;
    //    reloadEvent.Notify();
    //    cameraShake.PlayAimAnimation(weaponName, inAim);
    //}
}

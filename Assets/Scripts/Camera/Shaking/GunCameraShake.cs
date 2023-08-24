using UnityEngine;
using Cinemachine;

public class GunCameraShake : MonoBehaviour
{
    public CinemachineImpulseSource gunCameraShake;

    public void Awake()
    {
        gunCameraShake = GetComponent<CinemachineImpulseSource>();
    }

    public void GenerateRecoil()
    {
        gunCameraShake.GenerateImpulse(Camera.main.transform.forward);
    }
}

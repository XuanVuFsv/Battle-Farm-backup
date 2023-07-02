using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShootingInputData
{
    public ShootingInputData(ShootController _shootController, AmmoStats.ShootingHandleType _shootingHandleType, AmmoStatsController _ammoStatsController, Transform _raycastOrigin, Transform _fpsCameraTransform, GameEvent _hitEvent, CameraShake _cameraShake, Transform _bulletSpawnPoint, int _layerMask)
    {
        shootController = _shootController;
        shootingHandleType = _shootingHandleType;
        ammoStatsController = _ammoStatsController;
        raycastOrigin = _raycastOrigin;
        fpsCameraTransform = _fpsCameraTransform;
        hitEvent = _hitEvent;
        cameraShake = _cameraShake;
        bulletSpawnPoint = _bulletSpawnPoint;
        layerMask = _layerMask;
    }

    public ShootController shootController;

    public AmmoStats.ShootingHandleType shootingHandleType;
    public AmmoStatsController ammoStatsController;
    public Transform raycastOrigin;
    public Transform fpsCameraTransform;
    public GameEvent hitEvent;
    public CameraShake cameraShake;
    public Transform bulletSpawnPoint;
    public int layerMask;
}

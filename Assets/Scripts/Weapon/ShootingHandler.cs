using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingHandler : MonoBehaviour
{
    public class ShootingInputData
    {
        public ShootingInputData(AmmoStats.ShootingHandleType _shootingHandleType, AmmoStatsController _ammoStatsController, Transform _raycastOrigin, Transform _fpsCameraTransform, GameEvent _hitEvent, CameraShake _cameraShake, Transform _bulletSpawnPoint, int _layerMask)
        {
            shootingHandleType = _shootingHandleType;
            ammoStatsController = _ammoStatsController;
            raycastOrigin = _raycastOrigin;
            fpsCameraTransform = _fpsCameraTransform;
            hitEvent = _hitEvent;
            cameraShake = _cameraShake;
            bulletSpawnPoint = _bulletSpawnPoint;
            layerMask = _layerMask;
        }

        public AmmoStats.ShootingHandleType shootingHandleType;
        public AmmoStatsController ammoStatsController;
        public Transform raycastOrigin;
        public Transform fpsCameraTransform;
        public GameEvent hitEvent;
        public CameraShake cameraShake;
        public Transform bulletSpawnPoint;
        public int layerMask;
    }
    RaycastHit hit;
    //[SerializeField] string currentHitObject = "";

    public void ShootingHandle(ShootingInputData inputData)
    {
        if (inputData.shootingHandleType == AmmoStats.ShootingHandleType.Raycast)
        {
            RaycastHandle(inputData);
        }
        else if (inputData.shootingHandleType == AmmoStats.ShootingHandleType.InstantiateBullet)
        {
            InstantiateBulletHandle(inputData);
        }
    }

    void RaycastHandle(ShootingInputData inputData)
    {
        if (inputData.ammoStatsController.bulletCount == 1)
        {
            if (Physics.Raycast(inputData.raycastOrigin.position, inputData.fpsCameraTransform.forward, out hit, inputData.ammoStatsController.range, inputData.layerMask))
            {
                //hitEffectPrefab.transform.position = hit.point;
                //hitEffectPrefab.transform.forward = hit.normal;
                //hitEffectPrefab.Emit(5);

                PoolingManager.Instance.Get("Pool" + inputData.ammoStatsController.ammoStats.name + "Setup");
                inputData.hitEvent.Notify(hit);
                //currentHitObject = hit.collider.name;


                //Tracer here
                //Damage Handle here

                #region Minigame Test
                //tracer.transform.position = hit.point;
                //if (hit.transform.gameObject.tag == "Wall")
                //{
                //    GameObject wall = hit.transform.gameObject;
                //    WallSpawner.Instance.DestroyWall(wall.GetComponent<WallBehaviour>().index, hit);
                //}
                #endregion
            }
            //else tracer.transform.position += _fpsCameraTransform.forward * range;
        }
        else
        {
            List<RaycastHit> raycastHits = new List<RaycastHit>();

            //int i = 0;
            foreach (Vector3 pattern in inputData.ammoStatsController.ammoStats.bulletDirectionPattern)
            {
                Vector3 localDirection = Vector3.forward + pattern;
                Vector3 direction = inputData.fpsCameraTransform.TransformDirection(localDirection).normalized;

                if (Physics.Raycast(inputData.raycastOrigin.position, direction, out hit, inputData.ammoStatsController.range, inputData.layerMask))
                {
                    raycastHits.Add(hit);
                    PoolingManager.Instance.Get("Pool" + inputData.ammoStatsController.ammoStats.name + "Setup");
                    inputData.hitEvent.Notify(hit);
                }
            }
        }
    }

    void InstantiateBulletHandle(ShootingInputData inputData)
    {
        Vector3 direction = inputData.fpsCameraTransform.forward;

        if (inputData.ammoStatsController.ammoStats.fruitType == AmmoStats.FruitType.Star)
        {
            //Debug.Log("Shoot");
            Vector3 localDirection = Vector3.forward + inputData.cameraShake.GetCurrentPatternVector();
            direction = inputData.fpsCameraTransform.TransformDirection(localDirection).normalized;
        }

        GameObject newBullet = Instantiate(inputData.ammoStatsController.ammoStats.bulletObject, inputData.bulletSpawnPoint.position, Quaternion.identity);
        newBullet.GetComponent<BulletBehaviour>().TriggerBullet(inputData.ammoStatsController.ammoStats.name, inputData.ammoStatsController.force, direction);
    }
}

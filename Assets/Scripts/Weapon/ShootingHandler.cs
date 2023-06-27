using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingHandler : MonoBehaviour, IPrimaryWeaponStragety
{
    public ShootingInputData shootingInputData;
    public RaycastWeapon raycastWeapon;
    RaycastHit hit;

    private void Start()
    {
        raycastWeapon = GetComponent<RaycastWeapon>();
    }

    public void SetInputData(object _inputData)
    {
        shootingInputData = _inputData as ShootingInputData;
    }

    public ShootingInputData GetShootingInputData()
    {
        return shootingInputData;
    }

    public void HandleLeftMouseClick()
    {
        ShootingHandle();
    }

    public void HandleRightMouseClick()
    {

    }

    public void ShootingHandle()
    {
        if (shootingInputData.shootingHandleType == AmmoStats.ShootingHandleType.Raycast)
        {
            RaycastHandle();
        }
        else if (shootingInputData.shootingHandleType == AmmoStats.ShootingHandleType.InstantiateBullet)
        {
            InstantiateBulletHandle();
        }
    }

    void RaycastHandle()
    {
        if (shootingInputData.ammoStatsController.bulletCount == 1)
        {
            if (Physics.Raycast(shootingInputData.raycastOrigin.position, shootingInputData.fpsCameraTransform.forward, out hit, shootingInputData.ammoStatsController.range, shootingInputData.layerMask))
            {
                //hitEffectPrefab.transform.position = hit.point;
                //hitEffectPrefab.transform.forward = hit.normal;
                //hitEffectPrefab.Emit(5);

                PoolingManager.Instance.Get("Pool" + shootingInputData.ammoStatsController.ammoStats.name + "Setup");
                shootingInputData.hitEvent.Notify(hit);
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
            foreach (Vector3 pattern in shootingInputData.ammoStatsController.ammoStats.bulletDirectionPattern)
            {
                Vector3 localDirection = Vector3.forward + pattern;
                Vector3 direction = shootingInputData.fpsCameraTransform.TransformDirection(localDirection).normalized;

                if (Physics.Raycast(shootingInputData.raycastOrigin.position, direction, out hit, shootingInputData.ammoStatsController.range, shootingInputData.layerMask))
                {
                    raycastHits.Add(hit);
                    PoolingManager.Instance.Get("Pool" + shootingInputData.ammoStatsController.ammoStats.name + "Setup");
                    shootingInputData.hitEvent.Notify(hit);
                }
            }
        }
    }

    void InstantiateBulletHandle()
    {
        Vector3 direction = shootingInputData.fpsCameraTransform.forward;

        if (shootingInputData.ammoStatsController.ammoStats.fruitType == AmmoStats.FruitType.Star)
        {
            //Debug.Log("Shoot");
            Vector3 localDirection = Vector3.forward + shootingInputData.cameraShake.GetCurrentPatternVector();
            direction = shootingInputData.fpsCameraTransform.TransformDirection(localDirection).normalized;
        }

        GameObject newBullet = Instantiate(shootingInputData.ammoStatsController.ammoStats.bulletObject, shootingInputData.bulletSpawnPoint.position, Quaternion.identity);
        newBullet.GetComponent<BulletBehaviour>().TriggerBullet(shootingInputData.ammoStatsController.ammoStats.name, shootingInputData.ammoStatsController.force, direction);
    }
}
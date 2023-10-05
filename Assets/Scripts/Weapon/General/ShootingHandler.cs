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
        PlayAimAnimation();
    }

    public void ShootingHandle()
    {
        shootingInputData.shootController.ApplyAttackAnimation();
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
            WallBehaviour wall;
            if (Physics.Raycast(shootingInputData.raycastOrigin.position, shootingInputData.fpsCameraTransform.forward, out hit, shootingInputData.ammoStatsController.range, shootingInputData.layerMask))
            {
                //hitEffectPrefab.transform.position = hit.point;
                //hitEffectPrefab.transform.forward = hit.normal;
                //hitEffectPrefab.Emit(5);

                PoolingManager.Instance.Get("Pool" + shootingInputData.ammoStatsController.ammoStats.name + "Setup");
                shootingInputData.hitEvent.Notify(hit);
                shootingInputData.cameraShake.GenerateRecoil(shootingInputData.ammoStatsController.zoomType);
                //currentHitObject = hit.collider.name;
                wall = hit.transform.GetComponent<WallBehaviour>();
                if (wall != null)
                {
                    WallSpawner.Instance.DestroyWall(wall.index);
                }

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
            bool destroyedObstacle = false;
            WallBehaviour wall;
            foreach (Vector3 pattern in shootingInputData.ammoStatsController.ammoStats.bulletDirectionPattern)
            {
                Vector3 localDirection = Vector3.forward + pattern;
                Vector3 direction = shootingInputData.fpsCameraTransform.TransformDirection(localDirection).normalized;

                if (Physics.Raycast(shootingInputData.raycastOrigin.position, direction, out hit, shootingInputData.ammoStatsController.range, shootingInputData.layerMask))
                {
                    raycastHits.Add(hit);
                    PoolingManager.Instance.Get("Pool" + shootingInputData.ammoStatsController.ammoStats.name + "Setup");
                    shootingInputData.hitEvent.Notify(hit);
                    shootingInputData.cameraShake.GenerateRecoil(shootingInputData.ammoStatsController.zoomType);
                    wall = hit.transform.GetComponent<WallBehaviour>();
                    if (!destroyedObstacle && wall != null)
                    {
                        WallSpawner.Instance.DestroyWall(wall.index);
                        destroyedObstacle = true;
                    }
                }
            }
        }
    }

    void InstantiateBulletHandle()
    {
        Vector3 direction = shootingInputData.fpsCameraTransform.forward;

        if (shootingInputData.ammoStatsController.ammoStats.fruitType == AmmoStats.FruitType.Star)
        {
            MyDebug.Instance.Log("Shoot");
            Vector3 localDirection = Vector3.forward + shootingInputData.cameraShake.GetCurrentPatternVector();
            direction = shootingInputData.fpsCameraTransform.TransformDirection(localDirection).normalized;
        }

        GameObject newBullet = Instantiate(shootingInputData.ammoStatsController.ammoStats.bulletObject, shootingInputData.bulletSpawnPoint.position, Quaternion.identity);
        newBullet.GetComponent<BulletBehaviour>().TriggerBullet(shootingInputData.ammoStatsController.ammoStats.name, shootingInputData.ammoStatsController.force, direction);
        shootingInputData.cameraShake.GenerateRecoil(shootingInputData.ammoStatsController.zoomType);
    }

    public void PlayAimAnimation()
    {
        //MyDebug.Instance.Log("Handle Right Click");

        ShootController shootController = shootingInputData.shootController;
        if (!shootController.rigController) return;

        shootController.inAim = !shootController.inAim;
        shootController.aimEvent.Notify(shootController.inAim);

        if (shootController.inAim) shootController.aimEvent.Notify(shootingInputData.ammoStatsController.multiplierRecoilOnAim);

        shootController.rigController.SetBool("inAim", shootController.inAim);
    }
}
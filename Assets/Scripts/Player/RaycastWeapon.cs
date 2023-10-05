using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

// The RaycastWeapon as Context defines the interface of interest to clients.
public class RaycastWeapon : MonoBehaviour
{
    public IWeaponStragety weaponHandler;

    public AmmoStatsController ammoStatsController;
    public ShootController shootController;
    CameraShake cameraShake;
    GunCameraShake gunCameraShake;

    public GameObject bulletPrefab, magazineObject;

    public GameEvent hitEvent;

    public Transform raycastOrigin;
    public Transform fpsCameraTransform;

    public Transform bulletSpawnPoint, casingSpawnPoint;

    public ParticleSystem hitEffectPrefab;
    public ParticleSystem muzzleFlash;

    public AnimationClip weaponAnimation;

    WeaponStats weaponStats;
    RaycastHit hit;
    int layerMask;

    [SerializeField] string currentHitObject = "";
    [SerializeField] bool useDifferentClassForHandleShooting = true;
    [SerializeField] bool noStragety = true;

    #region
    //[Header("Weapon Sway (not used)")]
    //Enables weapon sway
    //bool weaponSway;
    //float swayAmount = 0.02f;
    //float maxSwayAmount = 0.06f;
    //float swaySmoothValue = 4.0f;
    #endregion

    private void Start()
    {
        cameraShake = GetComponent<CameraShake>();
        //ammoStatsController = GetComponent<AmmoStatsController>();
        weaponStats = GetComponent<WeaponStatsController>().weaponStats;

        fpsCameraTransform = Camera.main.transform;
        layerMask = ~(1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("Ignore Player") | 1 << LayerMask.NameToLayer("Only Player"));

        if (gameObject.activeInHierarchy) SetAsWeaponStrategy();

        #region
        //gunCameraShake = GetComponent<GunCameraShake>();
        //hitEffectPrefab = Instantiate(ammoStatsController.hitEffectPrefab, transform);
        //hitEffectPrefab.gameObject.layer = LayerMask.NameToLayer("Default");
        #endregion
    }

    public async UniTaskVoid SetAsInputData()
    {
        //MyDebug.Instance.Log(gameObject.name);
        //MyDebug.Instance.Log("Wait ammoStatController instance created");
        await UniTask.WaitUntil(() => ammoStatsController.ammoStats != null);
        //if (ammoStatsController == null) await UniTask.Yield();
        //MyDebug.Instance.Log(ammoStatsController);
        ShootingInputData shootingInputData = new ShootingInputData(shootController, ammoStatsController.ammoStats.shootingHandleType, ammoStatsController, raycastOrigin, fpsCameraTransform, hitEvent, cameraShake, bulletSpawnPoint, layerMask);
        weaponHandler.SetInputData(shootingInputData);
    }

    public void SetAsWeaponStrategy()
    {
        if (gameObject.activeInHierarchy == false) return;

        if (GetComponent<ShootingHandler>())
        {
            weaponHandler = GetComponent<ShootingHandler>();
            //MyDebug.Instance.Log(ammoStatsController);
            var _setInput = SetAsInputData();
        }
        else if (GetComponent<ActionHandler>())
        {
            weaponHandler = GetComponent<ActionHandler>();
            var setInput = SetAsInputData();
        }
        else
        {
            weaponHandler = gameObject.AddComponent<ActionHandler>();
        }
    }

    /// <summary>
    /// Run this method to shoot and select what type of bullet <see cref="AmmoStats.ShootingHandleType"/> and how to bullet interact to other objects
    /// </summary>
    public void HandleLeftMouseClick()
    {
        if (muzzleFlash) muzzleFlash.Emit(1);

        //cameraShake.GenerateRecoil(ammoStatsController.ammoStats.zoomType);
        //if (ammoStatsController.ammoStats.zoomType == AmmoStats.ZoomType.HasScope) cameraShake.GenerateRecoil(true);
        //else
        //{
        //    cameraShake.GenerateRecoil();
        //}

        #region Spawn bullet object and tracer
        //Spawn casing prefab at spawnpoint
        //Instantiate(weaponStats.casingPrefab,
        //    casingSpawnPoint.position,
        //    casingSpawnPoint.rotation);

        //Spawn bullet from bullet spawnpoint
        //Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, Quaternion.LookRotation(fpsCameraTransform.transform.forward));

        //var tracer = Instantiate(weaponStats.bulletTracer, raycastOrigin.position, Quaternion.identity);
        //tracer.AddPosition(bulletSpawnPoint.position);
        #endregion

        if (useDifferentClassForHandleShooting)
        {
            weaponHandler.HandleLeftMouseClick();
            //MyDebug.Instance.Log("Handle Left Mouse Click");
        }
        else ShootingHandle();
    }

    /// <summary>
    /// Run this method to aim. An ammo type can aim or not and how a gun aim with different ammo will depend on their ammo type. Check this:
    /// <see cref="AmmoStats.FruitType"></see>
    /// </summary>
    public void HandleRightMouseClick()
    {
        weaponHandler.HandleRightMouseClick();
    }

    public void StopFiring()
    {
        cameraShake.ResetRecoil();
    }

    void ShootingHandle()
    { 
        if(ammoStatsController.ammoStats.shootingHandleType == AmmoStats.ShootingHandleType.Raycast)
        {
            RaycastHandle();
        }
        else if (ammoStatsController.ammoStats.shootingHandleType == AmmoStats.ShootingHandleType.InstantiateBullet)
        {
            InstantiateBulletHandle();
        }
    }

    void RaycastHandle()
    {
        if (ammoStatsController.bulletCount == 1)
        {
            if (Physics.Raycast(raycastOrigin.position, fpsCameraTransform.forward, out hit, ammoStatsController.range, layerMask))
            {
                //hitEffectPrefab.transform.position = hit.point;
                //hitEffectPrefab.transform.forward = hit.normal;
                //hitEffectPrefab.Emit(5);

                PoolingManager.Instance.Get("Pool" + ammoStatsController.ammoStats.name + "Setup");
                hitEvent.Notify(hit);
                currentHitObject = hit.collider.name;


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
            //else tracer.transform.position += fpsCameraTransform.forward * range;
        }
        else
        {
            List<RaycastHit> raycastHits = new List<RaycastHit>();

            //int i = 0;
            foreach (Vector3 pattern in ammoStatsController.ammoStats.bulletDirectionPattern)
            {
                Vector3 localDirection = Vector3.forward + pattern;
                Vector3 direction = fpsCameraTransform.TransformDirection(localDirection).normalized;

                if (Physics.Raycast(raycastOrigin.position, direction, out hit, ammoStatsController.range, layerMask))
                {
                    raycastHits.Add(hit);
                    PoolingManager.Instance.Get("Pool" + ammoStatsController.ammoStats.name + "Setup");
                    hitEvent.Notify(hit);
                }
            }
        }
    }

    void InstantiateBulletHandle()
    {
        Vector3 direction = fpsCameraTransform.forward;

        if (ammoStatsController.ammoStats.fruitType == AmmoStats.FruitType.Star)
        {
            //MyDebug.Instance.Log("Shoot");
            Vector3 localDirection = Vector3.forward + cameraShake.GetCurrentPatternVector();
            direction = fpsCameraTransform.TransformDirection(localDirection).normalized;
        }

        GameObject newBullet = Instantiate(ammoStatsController.ammoStats.bulletObject, bulletSpawnPoint.position, Quaternion.identity);
        newBullet.GetComponent<BulletBehaviour>().TriggerBullet(ammoStatsController.ammoStats.name, ammoStatsController.force, direction);
    }

    public CameraShake GetCameraShake()
    {
        return cameraShake;
    }

    public GunCameraShake GetGunCameraShake()
    {
        return gunCameraShake;
    }

    private void OnDrawGizmos()
    {
        if (Application.isEditor) return;
        Gizmos.color = Color.green;
        Vector3 direction = fpsCameraTransform.forward;

        if (ammoStatsController.ammoStats.fruitType == AmmoStats.FruitType.Star)
        {
            Vector3 localDirection = Vector3.forward + cameraShake.GetCurrentPatternVector();
            direction = fpsCameraTransform.TransformDirection(localDirection).normalized;
        }
        //foreach (Vector3 pattern in currentShootingMechanic.bulletDirectionPattern)
        //{
        //    Vector3 localDirection = Vector3.forward + pattern;
        //    Vector3 direction = fpsCameraTransform.TransformDirection(localDirection).normalized;

        //    Gizmos.DrawRay(raycastOrigin.position, direction);
        //}
        Gizmos.DrawRay(raycastOrigin.position, direction);
        Gizmos.DrawLine(raycastOrigin.position, raycastOrigin.position + direction * ammoStatsController.range);
    }

    private void OnEnable()
    {
        //shootController.raycastWeapon.SetWeaponStrategy();
    }
}

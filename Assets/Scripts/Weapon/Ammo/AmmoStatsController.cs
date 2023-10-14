using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AmmoStatsController : MonoBehaviour
{
    public AmmoStats ammoStats;

    public AmmoStats.ZoomType zoomType;

    public int dropOffDsitance;

    public int decreseDamageRate;
    public int damageHead;
    public int damageBody;
    public int damageArmsLegs;
    public int range;
    public int bulletCount;
    public int force;
    public int amplitudeGainImpulse;

    public float fireRate;
    public float reloadSpeed;
    public float multiplierRecoilOnAim;
    public float multiplierForAmmo;

    public bool hasAssignAmmoData = false;

    public RaycastWeapon raycastWeapon;
    public WeaponStatsController weaponStatsController;
    public ParticleSystem hitEffectPrefab;
    public GameObject trailTracer;
    public GameObject bulletObject;

    public WaitForSeconds reloadTimer;

    [SerializeField]
    CameraShake cameraShake;

    // Start is called before the first frame update
    void Awake()
    {

    }

    private void Start()
    {
        cameraShake = gameObject.GetComponent<CameraShake>();
        raycastWeapon = GetComponent<RaycastWeapon>();
        weaponStatsController = GetComponent<WeaponStatsController>();

        if (ammoStats)
        {
            AssignAmmotData();
        }
    }

    void SetAmmoStats(AmmoStats newAmmoStats)
    {
        ammoStats = newAmmoStats;
    }

    public void AssignAmmotData()
    {
        //if (hasAssignAmmoData) return;
        //GetComponent<RaycastWeapon>().currentShootingMechanic = ammoStats.shootingMechanic;

        zoomType = ammoStats.zoomType;

        dropOffDsitance = ammoStats.dropOffDsitance;
        decreseDamageRate = ammoStats.decreseDamageRate;

        damageHead = ammoStats.damageHead;
        damageBody = ammoStats.damageBody;
        damageArmsLegs = ammoStats.damageArmsLegs;
        range = ammoStats.range;
        bulletCount = ammoStats.bulletCount;
        force = ammoStats.force;
        amplitudeGainImpulse = ammoStats.amplitudeGainImpulse;
        multiplierRecoilOnAim = ammoStats.multiplierRecoilOnAim;
        multiplierForAmmo = ammoStats.multiplierForAmmo;

        fireRate = ammoStats.fireRate;
        reloadSpeed = ammoStats.reloadSpeed;
        reloadTimer = new WaitForSeconds(reloadSpeed);

        hitEffectPrefab = ammoStats.hitEffectPrefab;
        trailTracer = ammoStats.trailTracer;
        bulletObject = ammoStats.bulletObject;

        cameraShake.AssignRecoilPattern(ammoStats.recoildPattern);

        weaponStatsController.ammoInMagazine = ammoStats.ammoAllowedInMagazine;
        raycastWeapon.SetAsWeaponStrategy();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AmmoStatsController : MonoBehaviour
{
    public AmmoStats ammoStats;

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

    public ParticleSystem hitEffectPrefab;
    public TrailRenderer bulletTracer;
    public GameObject bulletObject;

    public Sprite artwork;

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
        bulletTracer = ammoStats.bulletTracer;
        bulletObject = ammoStats.bulletObject;

        artwork = ammoStats.artwork;

        //hasAssignAmmoData = true;

        cameraShake.AssignRecoilPattern(ammoStats.recoildPattern);

        GetComponent<WeaponStatsController>().ammoInMagazine = ammoStats.ammoAllowedInMagazine;
    }
}
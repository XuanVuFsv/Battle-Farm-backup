using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class WeaponStatsController: MonoBehaviour
{
    public WeaponStats weaponStats;
    public ActiveWeapon.WeaponSlot weaponSlot;
    public AmmoStatsController currentAmmoStatsController;
    public GunCameraController gunCamera;
    [SerializeField]
    AmmoPickup ammunitionChestPicked, defaultAmmoOnStart;
    [SerializeField]
    CameraShake cameraShake;
    [SerializeField]
    GameEvent pickAmmoEvent;

    public string weaponName;
    public int currentAmmo, remainingAmmo, ammoInMagazine;
    public bool autoReload = true;
    public bool ofActiveWeapon = false;
    
    bool outOfAmmo; //out of ammo in magazine
    bool hasRun = false;

    //private bool notContainAmmo; //out of ammo in bag and can't refill ammo (magazine still can contain ammo or not)

    //public float penetrattionThickness;
    //public int dropOffDsitance;

    //public int decreseDamageRate;
    //public int damageHead;
    //public int damageBody;
    //public int damageArmsLegs;

    //public float fireRate;
    //public float reloadSpeed;

    //public ParticleSystem hitEffectPrefab;
    public GameObject magazineObject;
    public AnimationClip weaponAnimation;

    //public WaitForSeconds reloadTimer;
    //public Sprite artwork;
    //public Transform casingPrefab;
    //public TrailRenderer bulletTracer;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(transform.parent.name);
        //reloadTimer = new WaitForSeconds(reloadSpeed);
        //Resource Load
        //Debug.Log("Create a WeaponStatsController instance");

        cameraShake = gameObject.GetComponent<CameraShake>();
        if (!currentAmmoStatsController) currentAmmoStatsController = GetComponent<AmmoStatsController>();
        Invoke(nameof(OnStart), 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStart()
    {
        Debug.Log(hasRun + "from " + defaultAmmoOnStart.name);
        if (hasRun) return;
        SetupAmmoStats(defaultAmmoOnStart);
        hasRun = true;
    }

    public AmmoPickup GetDefaultAmmoOnStart()
    {
        return defaultAmmoOnStart;
    }

    public void SetupWeaponStats(WeaponStats weaponStats)
    {
        //Debug.Log("SetupWeaponStats");
        UpdateAmmoState();
        weaponName = weaponStats.name;
        weaponSlot = weaponStats.weaponSlot;
        //ammoInMagazine = weaponStats.magazine;
        //fireRate = weaponStats.fireRate;
        //reloadSpeed = weaponStats.reloadSpeed;
        weaponAnimation = weaponStats.weaponAnimation;
        ofActiveWeapon = true;
    }

    public void SetupAmmoStats(AmmoPickup ammoPickup)
    {
        Debug.Log(ammoPickup);
        //if (!currentAmmoStatsController) Debug.Log("currentAmmoStatsController null " + transform.parent.name);
        //Debug.Log(ammoPickup.name);

        if (!currentAmmoStatsController.ammoStats)
        {

            //Debug.Log("Add to null " + transform.parent.name);
            currentAmmoStatsController.ammoStats = ammoPickup.ammoStats;
            currentAmmoStatsController.AssignAmmotData();

            if (ofActiveWeapon)
            {
                //Debug.Log("Set" + currentAmmoStatsController.multiplierForAmmo);
                gunCamera.SetMultiplier(currentAmmoStatsController.multiplierForAmmo);
            }

            ammoPickup.AttachAmmoToObject(transform, false);

            pickAmmoEvent.Notify(currentAmmoStatsController.amplitudeGainImpulse);
            pickAmmoEvent.Notify(currentAmmoStatsController.multiplierRecoilOnAim);

            SetNewAmmoCount(ammoPickup);
            ammunitionChestPicked = ammoPickup;
        }
        else if (currentAmmoStatsController.ammoStats.fruitType == ammoPickup.ammoStats.fruitType)
        {
            //Debug.Log("Add same ammo");

            AddAmmo(ammoPickup.ammoContain);

            if (!ammunitionChestPicked)
            {
                ammoPickup.AttachAmmoToObject(transform, false);
                ammunitionChestPicked = ammoPickup;
            }
            else Destroy(ammoPickup.gameObject);

            ammoPickup.ammoContain = 0;
        }
        else
        {
            //Debug.Log("Add new ammo");

            if (ammunitionChestPicked)
            {
                ammunitionChestPicked.DetachAmmoToObject(null, true);
            }
            ammunitionChestPicked.ammoContain = InventoryController.Instance.GetItem(ammunitionChestPicked.ammoStats).count;

            currentAmmoStatsController.ammoStats = ammoPickup.ammoStats;
            ammoPickup.AttachAmmoToObject(transform, false);
            currentAmmoStatsController.AssignAmmotData();

            pickAmmoEvent.Notify(currentAmmoStatsController.amplitudeGainImpulse);
            pickAmmoEvent.Notify(currentAmmoStatsController.multiplierRecoilOnAim);

            SetNewAmmoCount(ammoPickup);
            ammunitionChestPicked = ammoPickup;
        }
    }

    void SetNewAmmoCount(AmmoPickup ammoPickup)
    {
        currentAmmo = 0;
        outOfAmmo = true;

        InventoryController.Instance.AddNewAmmoToInventory(ammoPickup.ammoStats, ammoPickup.ammoContain);
        remainingAmmo = ammoPickup.ammoContain;
        ammoInMagazine = ammoPickup.ammoStats.ammoAllowedInMagazine;

        UpdateAmmoUI();
    }

    public void UseAmmo(int ammo)
    {
        currentAmmo += ammo;
        InventoryController.Instance.GetCurrentItem().AddAmmo(ammo);
        //remainingAmmo = InventoryController.Instance.GetCurrentItem().count - currentAmmo; //ammo in inventory or bag

        if (currentAmmo == 0)
        {
            outOfAmmo = true;
        }

        UpdateAmmoUI();
    }

    public void UpdateAmmoAfterReload()
    {
        int neededAmmo = ammoInMagazine - currentAmmo; //ammo need to fill full magazine

        if (neededAmmo <= remainingAmmo)
        {
            currentAmmo = ammoInMagazine;
            remainingAmmo -= neededAmmo; //ammo in inventory or bag
            outOfAmmo = false;
        }
        else
        {
            currentAmmo += remainingAmmo;
            remainingAmmo = 0;
            outOfAmmo = false;
        }

        UpdateAmmoUI();
    }

    public void UpdateAmmoState()
    {
        if (currentAmmo > 0) outOfAmmo = false;
        else outOfAmmo = true;
    }

    public void UpdateAmmoUI()
    {
        WeaponSystemUI.Instance.UpdateAmmo(currentAmmo, remainingAmmo);
    }

    public void UpdateInventoryUI()
    {
        
    }

    public void AddAmmo(int ammo)
    {
        InventoryController.Instance.GetCurrentItem().AddAmmo(ammo);
        remainingAmmo = InventoryController.Instance.GetCurrentItem().count - currentAmmo;
    }

    public bool IsOutOfAmmo()
    {
        return outOfAmmo;
    }

    public bool IsContainAmmo()
    {
        return remainingAmmo > 0;
    }

    public bool IsFullMagazine()
    {
        return currentAmmo == ammoInMagazine;
    }
}
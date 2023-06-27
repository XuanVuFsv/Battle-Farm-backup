using System.Collections;
using UnityEngine;
//using Cysharp.Threading.Tasks;
//using System;

public class ShootController : MonoBehaviour
{
    public static class ShootingState
    {
        public static string None = "";
        public static string Aim = "Aim ";
    }

    [Header("Shooting")]
    public ActiveWeapon activeWeapon;
    public WeaponStatsController currentWeaponStatsController;
    public bool isFire;
    public RaycastWeapon raycastWeapon;

    [Header("Aiming")]
    [SerializeField]
    private PlayerAim playerAim;

    private InputController inputController;

    [Header("Reload")]
    public Animator rigController;
    public WeaponAnimationEvents animationEvents;
    public GameObject leftHand, magazineObject, magazineHand;
    private Vector3 newMagazineLocalPosition;
    private Vector3 newMagazineLocalEulerAngles;

    [Header("Events")]
    [SerializeField]
    GameEvent aimEvent;
    [SerializeField]
    GameEvent fireEvent;
    [SerializeField]
    GameEvent reloadEvent;

    [Header("Shooting Information")]
    private float lastFired;
    public bool isReloading = false;
    public bool inAim = false;
    public int shootingTime = 0;

    #region Advance Settings for muzzle and flash effect
    //[Header("Muzzleflash Settings")]
    //public ParticleSystem sparkParticles;
    //public ParticleSystem muzzleParticles;
    //public int maxRandomValue = 5;
    //public int minSparkEmission = 1;
    //public int maxSparkEmission = 7;
    //public bool randomMuzzleflash = false;
    //[Range(2, 25)]
    //public bool enableMuzzleflash = true;
    //public bool enableSparks = true;

    //private int minRandomValue = 1;
    //private int randomMuzzleflashValue;

    //[Header("Muzzleflash Light Settings")]
    //public Light muzzleflashLight;
    //public float lightDuration = 0.02f;
    #endregion

    #region Testing
    //float time = 0;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Created");

        animationEvents.weaponAnimationEvent.AddListener(OnAnimationEvent);
        inputController = GetComponent<InputController>();
        playerAim = GetComponent<PlayerAim>();
        activeWeapon = GetComponent<ActiveWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        //Out of ammo check
        CheckOuOfAmmo();

        //Aim handling
        RightMouseBehaviourHandle();

        //Fire handling
        LeftMouseBehaviourHandle();

        //Reload handling
        ReloadHandle();
    }

    void CheckOuOfAmmo()
    {
        if (currentWeaponStatsController.IsOutOfAmmo())
        {
            //Toggle bool
            isFire = false;
        }
    }

    void RightMouseBehaviourHandle()
    {
        if (raycastWeapon.weaponHandler is IPrimaryWeaponStragety)
        {
            if (inputController.isAim && (activeWeapon.activeWeaponIndex == 0 || activeWeapon.activeWeaponIndex == 1) && !isReloading)
            {
                PlayAimAnimation();
            }
        }
    }

    void LeftMouseBehaviourHandle()
    {
        if (raycastWeapon.weaponHandler is IPrimaryWeaponStragety)
        {
            if (((inputController.isFire && activeWeapon.activeWeaponIndex == 0)
            || (inputController.isSingleFire && activeWeapon.activeWeaponIndex != 0))
            && !currentWeaponStatsController.IsOutOfAmmo() && !isReloading)
            {
                //Debug.Log("Shoot");
                //Shoot automatic
                if (Time.time - lastFired > 1 / currentWeaponStatsController.currentAmmoStatsController.fireRate)
                {
                    //Debug.Log("Shoot");
                    lastFired = Time.time;

                    //Remove 1 bullet from ammo
                    currentWeaponStatsController.UpdateAmmo(-currentWeaponStatsController.currentAmmoStatsController.bulletCount);
                    shootingTime++;
                    //currentWeaponStatsController.UpdateAmmoUI();

                    isFire = true;
                    raycastWeapon.LeftMouseBehaviourHandle();

                    if (currentWeaponStatsController.currentAmmoStatsController.ammoStats.zoomType == AmmoStats.ZoomType.HasScope && inAim)
                    {
                        PlayAimAnimation();
                    }
                }
            }
        }

        if (inputController.isStopFire)
        {
            isFire = false;
            //rigController.SetBool("inAttack", inputController.isFire);
            raycastWeapon.StopFiring();
        }
    }

    void ReloadHandle()
    {
        if (((inputController.isReload && !currentWeaponStatsController.IsFullMagazine())
            || (currentWeaponStatsController.autoReload && currentWeaponStatsController.IsOutOfAmmo()))
            && currentWeaponStatsController.weaponSlot != ActiveWeapon.WeaponSlot.Melee
            && !isReloading
            && currentWeaponStatsController.IsContainAmmo())
        {
            //Debug.Log("Reload " + inputController.isReload + currentWeaponStatsController.IsFullMagazine() + currentWeaponStatsController.IsOutOfAmmo() + isReloading + currentWeaponStatsController.IsContainAmmo());
            StartCoroutine(Reload());
        }
    }

    //Reload handling
    IEnumerator Reload()
    {
        if (activeWeapon.activeWeaponIndex == (int)ActiveWeapon.WeaponSlot.Primary)
        {
            Debug.Log("Reload");

            rigController.SetTrigger("ReloadAK");
            rigController.SetBool("reloading", true);
            rigController.SetBool("inAim", false);
        }

        isReloading = true;
        //Debug.Log(isReloading);
        if (inAim)
        {
            inAim = false; 
            aimEvent.Notify(inAim);
            //aimEvent.Notify(1f);
        }

        yield return currentWeaponStatsController.currentAmmoStatsController.reloadTimer;

        //Restore ammo when reloading
        UpdateEndedReloadStats(true);
    }

    public void UpdateEndedReloadStats(bool ended)
    {
        if (ended)
        {
            currentWeaponStatsController.UpdateAmmoUI();

            if (activeWeapon.activeWeaponIndex == (int)ActiveWeapon.WeaponSlot.Primary)
            {
                Debug.Log("Reload");
                rigController.SetTrigger("ReloadAK");
            }
        }
        rigController.SetBool("reloading", false);
    }

    public void PlayAimAnimation()
    {
        if (!rigController) return;
        inAim = !inAim;
        aimEvent.Notify(inAim);

        if (inAim) aimEvent.Notify(currentWeaponStatsController.currentAmmoStatsController.multiplierRecoilOnAim);

        rigController.SetBool("inAim", inAim);
    }

    public void ApplyAimingAttributes()
    {

    }

    //Apply aim value to reference class. Affect to other attributes. Ex: recoild pattern, animation, player's movement speed
    public void ApllyAimValue(float val)
    {
        //Debug.Log($"Apply this value: {val} when aim");
    }

    #region Animation Events Handling
    void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "detach_magazine":
                DeTachMagazine();
                Debug.Log("Detach");
                break;
            case "drop_magazine":
                Debug.Log("Drop");
                DropMagazine();
                break;
            case "refill_magazine":
                Debug.Log("Refill");
                RefillMagazine();
                break;
            case "attach_magazine":
                Debug.Log("Attach");
                AttachMagazine();
                break;
            case "take_new_magazine":
                Debug.Log("Take new");
                TakeNewMagazine();
                break;
        }
    }

    void DeTachMagazine()
    {
        if(!magazineHand) magazineHand = Instantiate(magazineObject, leftHand.transform, true);

        newMagazineLocalPosition = magazineHand.transform.localPosition;
        newMagazineLocalEulerAngles = magazineHand.transform.localEulerAngles;

        magazineObject.SetActive(false);
    }

    void DropMagazine()
    {
        magazineHand.GetComponent<Rigidbody>().isKinematic = false;
    }

    void TakeNewMagazine()
    {
        magazineHand.GetComponent<Rigidbody>().isKinematic = true;
        magazineHand.transform.localPosition = newMagazineLocalPosition;
        magazineHand.transform.localEulerAngles = newMagazineLocalEulerAngles;
    }

    void RefillMagazine()
    {
        currentWeaponStatsController.UpdateAmmoAfterReload();
    }

    public void AttachMagazine()
    {
        Debug.Log("AttachMagazine");
        if (magazineObject != null) magazineObject.SetActive(true);
        isReloading = false;
        Destroy(magazineHand);
    }

    public void ResetMagazine()
    {
        if (magazineHand) Destroy(magazineHand);
    }
    #endregion
}
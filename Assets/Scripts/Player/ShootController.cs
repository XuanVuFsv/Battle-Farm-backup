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

    public InputController inputController;

    [Header("Reload")]
    public Animator rigController;
    public WeaponAnimationEvents animationEvents;
    public GameObject leftHand, magazineObject, magazineHand;
    private Vector3 newMagazineLocalPosition;
    private Vector3 newMagazineLocalEulerAngles;

    [Header("Events")]
    public GameEvent aimEvent;
    [SerializeField]
    GameEvent fireEvent;
    [SerializeField]
    GameEvent reloadEvent;

    [Header("Shooting Information")]
    private float lastFired;
    public bool isReloading = false;
    public bool inAim = false;
    public bool readyToFire = true;
    public int shootingTime = 0;

    int frame = 0;

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
        //MyDebug.Instance.Log("Created");

        animationEvents.weaponAnimationEvent.AddListener(OnAnimationEvent);
        inputController = GetComponent<InputController>();
        playerAim = GetComponent<PlayerAim>();
        activeWeapon = GetComponent<ActiveWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        frame++;

        //Out of ammo check
        CheckOuOfAmmo();

        RightMouseBehaviourHandle();

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

    //void MouseHandle()
    //{
    //    if (inputController.isAim)
    //    {
    //        //Aim handling
    //        RightMouseBehaviourHandle();
    //    }
    //    else
    //    {
    //        //Fire handling
    //        LeftMouseBehaviourHandle();
    //    }
    //}

    void RightMouseBehaviourHandle()
    {
        if (inputController.isAim)
        {
            if (raycastWeapon.weaponHandler is IPrimaryWeaponStragety && !isReloading)
            {
                //MyDebug.Instance.Log("Handle Right Click");
                //MyDebug.Instance.Log(frame);
                raycastWeapon.HandleRightMouseClick();
            }
            else if (raycastWeapon.weaponHandler is IHandGunWeaponStragety)
            {
                raycastWeapon.HandleRightMouseClick();
            }
        }
    }

    void LeftMouseBehaviourHandle()
    {
        #region Logic for PrimaryWeapon
        if (raycastWeapon.weaponHandler is IPrimaryWeaponStragety)
        {
            if (((inputController.isFire && activeWeapon.activeWeaponIndex == 0)
            || (inputController.isSingleFire && activeWeapon.activeWeaponIndex != 0))
            && !currentWeaponStatsController.IsOutOfAmmo() && !isReloading)
            {
                //MyDebug.Instance.Log("Shoot");
                //Shoot automatic
                if (Time.time - lastFired > 1 / currentWeaponStatsController.currentAmmoStatsController.fireRate)
                {
                    readyToFire = true;
   
                    //MyDebug.Instance.Log("Shoot");
                    lastFired = Time.time;

                    //Remove 1 bullet from ammo
                    currentWeaponStatsController.UpdateAmmo(-currentWeaponStatsController.currentAmmoStatsController.bulletCount);
                    shootingTime++;
                    //currentWeaponStatsController.UpdateAmmoUI();

                    isFire = true;
                    raycastWeapon.HandleLeftMouseClick();

                    if (currentWeaponStatsController.currentAmmoStatsController.ammoStats.zoomType == AmmoStats.ZoomType.HasScope && inAim)
                    {
                        //MyDebug.Instance.Log("Handle Right Click");
                        //MyDebug.Instance.Log(frame);
                        raycastWeapon.HandleRightMouseClick();
                    }
                }
                else
                {
                    DeactivateShooting();
                }
            }

            if (inputController.isStopFire)
            {
                isFire = false;
                //rigController.SetBool("inAttack", inputController.isFire);
                raycastWeapon.StopFiring();
                DeactivateShooting();
            }
        }
        else if (raycastWeapon.weaponHandler is IHandGunWeaponStragety)
        {
            if (inputController.isFire && activeWeapon.activeWeaponIndex == 1) raycastWeapon.HandleLeftMouseClick();
        }
        #endregion
    }

    void DeactivateShooting()
    {
        readyToFire = false;
        rigController.SetBool("inAttack", false);
    }

    void ReloadHandle()
    {
        if (((inputController.isReload && !currentWeaponStatsController.IsFullMagazine())
            || (currentWeaponStatsController.autoReload && currentWeaponStatsController.IsOutOfAmmo()))
            && currentWeaponStatsController.weaponSlot != ActiveWeapon.WeaponSlot.Melee
            && !isReloading
            && currentWeaponStatsController.IsContainAmmo())
        {
            //MyDebug.Instance.Log("Reload " + inputController.isReload + currentWeaponStatsController.IsFullMagazine() + currentWeaponStatsController.IsOutOfAmmo() + isReloading + currentWeaponStatsController.IsContainAmmo());
            StartCoroutine(Reload());
        }
    }

    //Reload handling
    IEnumerator Reload()
    {
        if (activeWeapon.activeWeaponIndex == (int)ActiveWeapon.WeaponSlot.Primary)
        {
            MyDebug.Instance.Log("Reload");

            rigController.SetTrigger("ReloadAK");
            rigController.SetBool("reloading", true);
            rigController.SetBool("inAim", false);
        }

        isReloading = true;
        MyDebug.Instance.Log(isReloading);
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
                MyDebug.Instance.Log("Reload");
                rigController.SetTrigger("ReloadAK");
            }
        }
        rigController.SetBool("reloading", false);
        RefillMagazine();
    }

    public void ApplyAttackAnimation()
    {
        //MyDebug.Instance.Log("Checking inAttack");
        if (activeWeapon.GetActiveWeaponPickup().weaponSlot == ActiveWeapon.WeaponSlot.Sidearm) rigController.SetBool("inAttack", inputController.isSingleFire);
        else rigController.SetBool("inAttack", true);
    }

    //public void PlayAimAnimation()
    //{
    //    if (!rigController) return;
    //    inAim = !inAim;
    //    aimEvent.Notify(inAim);

    //    if (inAim) aimEvent.Notify(currentWeaponStatsController.currentAmmoStatsController.multiplierRecoilOnAim);

    //    rigController.SetBool("inAim", inAim);
    //}

    public void ApplyAimingAttributes()
    {

    }

    //Apply aim value to reference class. Affect to other attributes. Ex: recoild pattern, animation, player's movement speed
    public void ApllyAimValue(float val)
    {
        //MyDebug.Instance.Log($"Apply this value: {val} when aim");
    }

    #region Animation Events Handling
    void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "detach_magazine":
                DeTachMagazine();
                MyDebug.Instance.Log("Detach event");
                break;
            case "drop_magazine":
                MyDebug.Instance.Log("Drop event");
                DropMagazine();
                break;
            case "refill_magazine":
                MyDebug.Instance.Log("Refill event");
                break;
            case "attach_magazine":
                MyDebug.Instance.Log("Attach event");
                AttachMagazine();
                break;
            case "take_new_magazine":
                MyDebug.Instance.Log("Take new event");
                TakeNewMagazine();
                break;
        }
    }

    void DeTachMagazine()
    {
        MyDebug.Instance.Log("Run Detach funtion");
        if(!magazineHand) magazineHand = Instantiate(magazineObject, leftHand.transform, true);

        newMagazineLocalPosition = magazineHand.transform.localPosition;
        newMagazineLocalEulerAngles = magazineHand.transform.localEulerAngles;

        magazineObject.SetActive(false);
    }

    void DropMagazine()
    {
        MyDebug.Instance.Log("Run Drop funtion");
        magazineHand.GetComponent<Rigidbody>().isKinematic = false;
    }

    void TakeNewMagazine()
    {
        MyDebug.Instance.Log("Run Take New funtion");
        magazineHand.GetComponent<Rigidbody>().isKinematic = true;
        magazineHand.transform.localPosition = newMagazineLocalPosition;
        magazineHand.transform.localEulerAngles = newMagazineLocalEulerAngles;
    }

    void RefillMagazine()
    {
        MyDebug.Instance.Log("Run Refill function");
        isReloading = false;
        currentWeaponStatsController.UpdateAmmoAfterReload();
    }

    public void AttachMagazine()
    {
        MyDebug.Instance.Log("Run AttachMagazine function");
        if (magazineObject != null) magazineObject.SetActive(true);
        Destroy(magazineHand);
    }

    public void ResetMagazine()
    {
        MyDebug.Instance.Log("Run ResetMagazine funtion");
        if (magazineHand) Destroy(magazineHand);
    }
    #endregion
}
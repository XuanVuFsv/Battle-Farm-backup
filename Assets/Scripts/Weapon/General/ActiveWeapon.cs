using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveWeapon : MonoBehaviour
{
    private static ActiveWeapon instance;

    public enum WeaponSlot
    {
        Primary = 0,
        Sidearm = 1,
        Melee = 2,
        Grenade = 3,
    }

    public enum WeaponAction
    {
        Throw = 0,
        Pickup = 1,
        Switch = 2,
        View = 3
    }

    public static WeaponPickup[] equippedWeapon = new WeaponPickup[4];

    public UnityEngine.Animations.Rigging.Rig handIk;
    public Cinemachine.CinemachineVirtualCamera playerCamera;

    public WeaponPickup defaultWeapon0, defaultWeapon1, defaultWeapon2, defaultWeapon3;
    public Transform[] weaponActivateSlots;
    public Transform weaponPivot;
    public Transform rightHandHolder, leftHandHolder;
    public Transform gunCamera;
    public List<WeaponPickup> triggerWeaponList = new List<WeaponPickup>();
    public List<AmmoPickup> triggerAmmoList = new List<AmmoPickup>();

    public float minDistanceToWeapon = 5;
    public int activeWeaponIndex = 3;
    public bool isHoldWeapon = false;

    [SerializeField]
    Animator rigController;
    [SerializeField]
    GunCameraController gunCameraController;
    [SerializeField]
    Transform[] equippedWeaponParent = new Transform[3];

    InputController inputController;
    ShootController shootController;
    Transform droppedWeapon;

    [SerializeField] int nearestWeaponIndex, nearestAmmoIndex;
    [SerializeField] string nearestWeapon, nearestAmmo;

    void MakeInstance()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else instance = this;
    }

    public static ActiveWeapon Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        shootController = GetComponent<ShootController>();
        inputController = GetComponent<InputController>();

        EquipWeapon(WeaponAction.Pickup, defaultWeapon0, true);
        SetupNewWeapon(defaultWeapon0.weaponStats);

        AttachWeapon(defaultWeapon1, weaponActivateSlots[1], 1);
        AttachWeapon(defaultWeapon2, weaponActivateSlots[2], 2);
        AttachWeapon(defaultWeapon3, weaponActivateSlots[3], 3);

        gunCameraController.SetHasScope(shootController.currentWeaponStatsController.GetDefaultAmmoOnStart().ammoStats.zoomType == AmmoStats.ZoomType.HasScope);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    //SceneManager.LoadScene("LoadingScene");
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}

        //if (GetNearestWeapon()) nearestWeapon = GetNearestWeapon().gameObject.transform.parent.name;
        //if (GetNearestAmmo()) nearestAmmo = GetNearestAmmo().gameObject.transform.name;

        rigController.SetBool("isHoldWeapon", isHoldWeapon);

        //if (Input.GetKeyDown(KeyCode.KeypadEnter)) Time.timeScale = 1;

        //AmmoPickupDetect();

        #region Drop and Pick Weapon
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    if (activeWeaponIndex != 2)
        //    {
        //        DropWeapon(WeaponAction.Throw, activeWeaponIndex);

        //        if (!GetWeapon(0) || !GetWeapon(1))
        //        {
        //            SwitchWeapon(equippedWeapon[2]);
        //        }
        //        else
        //        {
        //            if (activeWeaponIndex == 0) SwitchWeapon(equippedWeapon[1]);
        //            else SwitchWeapon(equippedWeapon[0]);
        //        }
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    PickWeapon();
        //}
        #endregion

        if (Input.GetKeyDown(KeyCode.E))
        {
            PickAmmo();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(equippedWeapon[0]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(equippedWeapon[1]);

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(equippedWeapon[2]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchWeapon(equippedWeapon[3]);
        }
    }

    public WeaponPickup GetWeaponPickupByIndex(int index)
    {
        return equippedWeapon[index];
    }

    public WeaponPickup GetActiveWeaponPickup()
    {
        return equippedWeapon[activeWeaponIndex];
    }

    public static WeaponPickup GetWeapon(int index)
    {
        if (index < 0 || index > 2)
        {
            return null;
        }
        return equippedWeapon[index];
    }

    void PickWeapon()
    {
        if (triggerWeaponList.Count == 0) return;

        WeaponPickup pickedWeapon;
        if (triggerWeaponList.Count > 1)
        {
            pickedWeapon = triggerWeaponList[nearestWeaponIndex];
        }
        else
        {
            pickedWeapon = triggerWeaponList[0];
        }

        if (!pickedWeapon.canPickup) return;

        triggerWeaponList.Remove(pickedWeapon);

        int pickedWeaponSlot = (int)pickedWeapon.weaponSlot;
        bool isExistWeaponSlot = GetWeapon(pickedWeaponSlot);
        DropWeapon(WeaponAction.Pickup, pickedWeaponSlot);
        EquipWeapon(WeaponAction.Pickup, pickedWeapon, false, isExistWeaponSlot);
        SetupNewWeapon(pickedWeapon.weaponStats);
    }

    void PickAmmo()
    {
        if (triggerAmmoList.Count == 0 || activeWeaponIndex == 4) return; // Only primary weapon can change ammo type

        AmmoPickup pickedAmmo = GetNearestAmmo();

        //if (triggerAmmoList.Count > 1)
        //{
        //    //if (!triggerAmmoList[nearestAmmoIndex])
        //    //{
        //    //    triggerAmmoList.RemoveAt(nearestAmmoIndex);
        //    //}
        //    pickedAmmo = GetNearestAmmo();
        //}
        //else
        //{
        //    pickedAmmo = triggerAmmoList[0];
        //}

        if (!pickedAmmo.canPickup) return;
        int weaponIndex = (int)pickedAmmo.ammoStats.weaponSlot;

        equippedWeapon[weaponIndex].GetComponent<WeaponStatsController>().SetupAmmoStats(pickedAmmo);
        triggerAmmoList.RemoveAt(nearestAmmoIndex);
        gunCameraController.SetHasScope(pickedAmmo.ammoStats.zoomType == AmmoStats.ZoomType.HasScope);
    }

    void SwitchWeapon(WeaponPickup activateWeapon)
    {
        //only run this funtion when player swich to a weapon not null and different weapon type
        if (activateWeapon)
        {
            if (activeWeaponIndex == (int)activateWeapon.weaponSlot) return;
        }
        else return;

        if (shootController.isReloading)
        {
            MyDebug.Instance.Log("Switch");
            shootController.AttachMagazine();
            shootController.StopAllCoroutines();
            shootController.StopCoroutine("Reload");
        }
        else shootController.ResetMagazine();

        shootController.UpdateEndedReloadStats(false);

        if (isHoldWeapon) DropWeapon(ActiveWeapon.WeaponAction.Switch, (int)equippedWeapon[activeWeaponIndex].weaponSlot);

        EquipWeapon(WeaponAction.Switch, activateWeapon, true);
        SetupNewWeapon(activateWeapon.weaponStats);

        rigController.SetInteger("weaponIndex", activeWeaponIndex);
        gunCameraController.SetHasScope(activateWeapon.GetComponent<AmmoStatsController>().ammoStats.zoomType == AmmoStats.ZoomType.HasScope);
    }

    public void EquipWeapon(WeaponAction action, WeaponPickup newWeapon, bool runAnimation, bool isExistWeaponSlot = false)
    {
        int weaponSlotIndex = (int)newWeapon.weaponSlot;

        equippedWeapon[weaponSlotIndex] = newWeapon;
        equippedWeaponParent[weaponSlotIndex] = equippedWeapon[weaponSlotIndex].transform.parent;

        bool sameWeaponSlot = (activeWeaponIndex == weaponSlotIndex);

        if (action == WeaponAction.Switch || runAnimation || isExistWeaponSlot)
        {
            //newWeapon.GetComponent<CameraShake>().Subscribe();
            shootController.currentWeaponStatsController = newWeapon.GetComponent<WeaponStatsController>();
            shootController.currentWeaponStatsController.currentAmmoStatsController = newWeapon.GetComponent<AmmoStatsController>();

            shootController.currentWeaponStatsController.UpdateAmmoUI();
            //shootController.currentWeaponStatsController.refferedToShootController = true;

            if (action == WeaponAction.Switch || runAnimation)
            {
                activeWeaponIndex = weaponSlotIndex;
                newWeapon.transform.parent.gameObject.SetActive(true);
            }

            if (sameWeaponSlot || runAnimation)
            {
                equippedWeaponParent[weaponSlotIndex].parent = weaponPivot;
                SetWeaponAnimation();
            }
        }

        if (activeWeaponIndex == weaponSlotIndex)
        {
            newWeapon.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            newWeapon.transform.parent.gameObject.SetActive(false);
        }

        if (action == WeaponAction.Pickup)
        {
            //Set current weapon and its parent
            newWeapon.GetComponent<CameraShake>().SetUpWeaponRecoilForNewWeapon(playerCamera, rigController);

            //UI, Layer, Physic
            if (newWeapon.weaponUI) newWeapon.weaponUI.gameObject.SetActive(false);
            SetupUtilities.SetLayers(equippedWeaponParent[weaponSlotIndex], "Local Player", "Local Player", "Effect");
            if (weaponSlotIndex != 2) equippedWeaponParent[weaponSlotIndex].gameObject.GetComponent<Rigidbody>().isKinematic = true;

            //Set parent for weapon, change noParent value and run animation
            newWeapon.noParent = false;
        }

        equippedWeaponParent[weaponSlotIndex].parent = weaponActivateSlots[weaponSlotIndex];
        equippedWeaponParent[weaponSlotIndex].localPosition = new Vector3(0, -0.5f, 0.5f);
        isHoldWeapon = true;

        //if (activeWeaponIndex == 2) holdWeapon.Notify(false);
        //else holdWeapon.Notify(true);
    }

    public void DropWeapon(WeaponAction action, int weaponSlotIndex)
    {
        bool isExsitWeaponSlot = true;

        //Don't run this function when don't hold any weapon or throw a null weapon
        if (!equippedWeapon[weaponSlotIndex])
        {
            isExsitWeaponSlot = false;
        }
        else
        {
            //equippedWeapon[weaponSlotIndex].GetComponent<CameraShake>().UnSubscribe();
            //shootController.currentWeaponStatsController.currentAmmoStatsController = null;
            shootController.currentWeaponStatsController.ofActiveWeapon = false;
            shootController.currentWeaponStatsController = null;

            //Switch
            if (action == WeaponAction.Switch)
            {
                rigController.SetFloat("multiplier", -1);
                rigController.Play("Base Layer.Equip " + equippedWeapon[activeWeaponIndex].weaponStats.name, 0, 0f);
                rigController.SetFloat("multiplier", 1);
                equippedWeaponParent[activeWeaponIndex].gameObject.SetActive(false);
                return;
            }
            //Throw or Pickup
            //Set parent for weapon and change noParent value, layers            
            equippedWeapon[weaponSlotIndex].GetComponent<CameraShake>().rigController = null;
            equippedWeapon[weaponSlotIndex].noParent = true;

            droppedWeapon = equippedWeaponParent[weaponSlotIndex].transform;

            SetupUtilities.SetLayers(equippedWeaponParent[weaponSlotIndex], "Ignore Player", "Only Player", null);

            if (weaponSlotIndex != 2)  equippedWeaponParent[weaponSlotIndex].GetComponent<Rigidbody>().isKinematic = false;
            if (weaponSlotIndex != 2) equippedWeaponParent[weaponSlotIndex].GetComponent<Rigidbody>().AddForce(transform.forward * 5, ForceMode.Impulse);

            isHoldWeapon = false;

            if (!equippedWeapon[weaponSlotIndex].weaponUI)
            {
                equippedWeapon[weaponSlotIndex].CreateWeaponUI();
            }
        }

        if (action == WeaponAction.Throw || (action == WeaponAction.Pickup && isExsitWeaponSlot))
        {
            droppedWeapon.gameObject.SetActive(true);
            droppedWeapon.parent = null;
            equippedWeaponParent[weaponSlotIndex] = null;
            equippedWeapon[weaponSlotIndex] = null;
        }
    }

    public void AttachWeapon(WeaponPickup attachedWeapon, Transform attachedWeaponParent, int attachedWeaponSlotIndex)
    {
        attachedWeapon.transform.parent.gameObject.SetActive(false);
        SetupUtilities.SetLayers(attachedWeapon.transform.parent, "Local Player", "Local Player", "Effect");
        attachedWeapon.noParent = false;

        attachedWeapon.GetComponent<CameraShake>().rigController = rigController;
        equippedWeapon[attachedWeaponSlotIndex] = attachedWeapon;
        equippedWeaponParent[attachedWeaponSlotIndex] = attachedWeapon.transform.parent;

        StartCoroutine(SetWeaponParent(attachedWeapon, attachedWeaponParent));
        //MyDebug.Instance.Log("OnStart " + attachedWeapon.transform.parent.name);
        attachedWeapon.GetComponent<WeaponStatsController>().OnStart();
    }

    public void SetupNewWeapon(WeaponStats weaponStats)
    {
        equippedWeapon[activeWeaponIndex].GetComponent<WeaponStatsController>().SetupWeaponStats(weaponStats);

        shootController.magazineObject = equippedWeapon[activeWeaponIndex].GetComponent<WeaponStatsController>().magazineObject;
        shootController.raycastWeapon = equippedWeapon[activeWeaponIndex].GetComponent<RaycastWeapon>();

        if (activeWeaponIndex == 2 || activeWeaponIndex == 3) gunCameraController.SetHoldWeaponAnimation(false, (int)weaponStats.weaponSlot);
        else gunCameraController.SetHoldWeaponAnimation(true, (int)weaponStats.weaponSlot);
    }

    void SetWeaponAnimation()
    {
        Vector3 droppedWeaponPosition = Vector3.zero;
        if (droppedWeapon)
        {
            droppedWeaponPosition = droppedWeapon.position;
            rigController.Rebind();
            droppedWeapon.position = droppedWeaponPosition; 
        }
        else rigController.Rebind();

        rigController.Play("Base Layer.Equip " + equippedWeapon[activeWeaponIndex].weaponStats.name, 0, 0f);
    }

    IEnumerator SetWeaponParent(WeaponPickup weapon, Transform weaponParent)
    {
        yield return new WaitForSeconds(defaultWeapon2.GetComponent<RaycastWeapon>().weaponAnimation.length);
        weapon.transform.parent.parent = weaponParent;
    }

    void OnTriggerEnter(Collider other)
    {
        WeaponPickup weaponPickup = other.GetComponent<WeaponPickup>();
        if (weaponPickup)
        {
            if(!triggerWeaponList.Contains(weaponPickup)) triggerWeaponList.Add(weaponPickup);
        }

        AmmoPickup ammoPickup = other.GetComponent<AmmoPickup>();
        if (ammoPickup && !ammoPickup.hasParent)
        {
            if (!triggerAmmoList.Contains(ammoPickup)) triggerAmmoList.Add(ammoPickup);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (triggerWeaponList.Contains(other.GetComponent<WeaponPickup>()))
        {
            //minDistanceToWeapon = 5;
            triggerWeaponList.Remove(other.GetComponent<WeaponPickup>());
        }

        if (triggerAmmoList.Contains(other.GetComponent<AmmoPickup>()))
        {
            //minDistanceToWeapon = 5;
            triggerAmmoList.Remove(other.GetComponent<AmmoPickup>());
        }
    }

    public WeaponPickup GetNearestWeapon()
    {
        WeaponPickup weapon = null;
        float min = 5;
        for (int i = 0; i < triggerWeaponList.Count; i++)
        {
            float distance = Vector3.Distance(triggerWeaponList[i].transform.parent.position, transform.position);
            if (distance <= min/* && triggerWeaponList[i].inWeaponViewport*/)
            {
                min = distance;
                weapon = triggerWeaponList[i];
                nearestWeaponIndex = i;
            }
        }
        return weapon;
    }

    public AmmoPickup GetNearestAmmo()
    {
        AmmoPickup ammo = null;
        float min = 5;
        for (int i = 0; i < triggerAmmoList.Count; i++)
        {
            float distance = Vector3.Distance(triggerAmmoList[i].transform.position, transform.position);
            if (distance <= min)
            {
                min = distance;
                ammo = triggerAmmoList[i];
                nearestAmmoIndex = i;
            }
        }
        return ammo;
    }

    public string GetCurrentWeaponName()
    {
        return equippedWeapon[activeWeaponIndex].weaponStats.name;
    }

    //[ContextMenu("Save Weapon Pose")]
    //void SaveWeaponPose()
    //{
    //    GameObjectRecorder recorder = new GameObjectRecorder(transform.GetChild(0).gameObject);
    //    recorder.BindComponentsOfType<Transform>(weaponPivot.gameObject, false);
    //    recorder.BindComponentsOfType<Transform>(equippedWeaponParent[activeWeaponIndex].gameObject, false);
    //    recorder.BindComponentsOfType<Transform>(gunCamera.gameObject, false);
    //    recorder.BindComponentsOfType<Transform>(leftHandHolder.gameObject, false);
    //    recorder.BindComponentsOfType<Transform>(rightHandHolder.gameObject, false);
    //    recorder.TakeSnapshot(0.0f);
    //    recorder.SaveToClip(equippedWeapon[activeWeaponIndex].GetComponent<RaycastWeapon>().weaponAnimation);
    //}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public RectTransform weaponUIPrefab, weaponUI;
    public WeaponStats weaponStats;
    public ActiveWeapon.WeaponSlot weaponSlot;
    public Vector3 viewPortPoint;
    public bool noParent = true;
    public bool canPickup = false;

    public bool inWeaponViewport;
    ActiveWeapon activeWeapon;

    static int standardLengthWeponName = 4;
    static float offsetPerOverLetter = 0.5f;

    private void Start()
    {
        weaponStats = GetComponent<WeaponStatsController>().weaponStats;
        weaponSlot = weaponStats.weaponSlot;
        if (noParent)
        {
            CreateWeaponUI();
        }
    }

    private void Update()
    {
        if (inWeaponViewport && weaponUI)
        {
            if (weaponSlot == ActiveWeapon.WeaponSlot.Primary)
            {
                weaponUI.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, -transform.parent.localEulerAngles.z);
            }
            else
            {
                weaponUI.GetComponent<RectTransform>().localEulerAngles = new Vector3(-transform.parent.localEulerAngles.x, 0, 0);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Camera.main.transform.position, transform.position);
    }

    private void OnTriggerStay(Collider other)
    {
        canPickup = true;
        if (other.CompareTag("Player") && noParent)
        {
            //viewPortPoint = Camera.main.WorldToViewportPoint(transform.position);
            if (activeWeapon == null) activeWeapon = other.GetComponent<ActiveWeapon>();

            //inWeaponViewport = WeaponInPickupViewPort();
            //inWeaponViewport = true;

            if (activeWeapon.triggerWeaponList.Count > 0)
            {
                if (!weaponUI)
                {
                    CreateWeaponUI();
                }

                if (activeWeapon.triggerWeaponList.Count == 1)
                {
                    ShowWeaponStats();
                }
                else
                {
                    if (this == activeWeapon.GetNearestWeapon())
                    {
                        ShowWeaponStats();
                    }
                    else
                    {
                        weaponUI.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                weaponUI.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && noParent)
        {
            activeWeapon = null;

            weaponUI.gameObject.SetActive(false);
            canPickup = false;
        }
    }

    bool WeaponInPickupViewPort()
    {
        if (viewPortPoint.z < 3.5f && Mathf.Abs(viewPortPoint.x - 0.5f) < 0.35f && Mathf.Abs(viewPortPoint.y - 0.5f) < 0.35f)
        {
            return true;
        }
        return false;
    }

    public void ShowWeaponStats()
    {
        weaponUI.gameObject.SetActive(true);
        weaponUI.GetChild(0).GetComponent<WeaponUI>().weaponName.text = weaponStats.name;
    }

    public void CreateWeaponUI()
    {
        weaponUI = Instantiate(weaponUIPrefab, transform.parent);
        weaponUI.localScale = CalcualteLocalScale(0.19f, 0.19f, 0.19f, transform.parent.localScale);
        int multiplier = weaponStats.name.Length - standardLengthWeponName;
        if (multiplier > 0) weaponUI.GetChild(0).GetComponent<WeaponUI>().panel.localPosition -= offsetPerOverLetter * multiplier * Vector3.right;

        weaponUI.gameObject.SetActive(false);
    }

    Vector3 CalcualteLocalScale(float x, float y, float z, Vector3 parentScale)
    {
        return new Vector3(x / parentScale.x * weaponUI.localScale.x,
                           y / parentScale.y * weaponUI.localScale.y,
                           z / parentScale.z * weaponUI.localScale.z);
    }
}

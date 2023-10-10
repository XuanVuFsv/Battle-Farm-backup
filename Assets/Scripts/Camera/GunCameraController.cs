using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GunCameraController : GameObserver
{
    [SerializeField] GameEvent aimEvent;
    [SerializeField] Animator vcamAnimator;
    [SerializeField] Animator crosshairAnimator;
    [SerializeField] Animator animator;
    [SerializeField] bool hasScope, isHoldWeapon;
    [SerializeField] int weaponIndex;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        AddGameEventToObserver(aimEvent);
    }

    public void SetHoldWeaponAnimation(bool _isHoldWeapon, int _weaponIndex)
    {
        //Debug.Log("Notified " + gEvent.GameEventName + " event to GunCameraController " + value);
        isHoldWeapon = _isHoldWeapon;
        weaponIndex = _weaponIndex;
        animator?.SetBool("isHoldWeapon", isHoldWeapon);
        animator?.SetInteger("weaponIndex", weaponIndex);

        //if (gEvent.GameEventName == "HoldWeapon")
        //{
        //    animator?.SetBool("isHoldWeapon", value);
        //}
        //else
        //{
        //    animator?.SetTrigger("Aim");
        //    vcamAnimator.SetTrigger("Aim");
        //    crosshairAnimator.SetBool("inAim", value);
        //}
    }

    public void SetMultiplier(float _multiplier)
    {
        animator?.SetFloat("multiplier", _multiplier);
    }

    public void SetHasScope(bool _hasScope)
    {
        hasScope = _hasScope;

        crosshairAnimator.SetBool("hasScope", hasScope);
        crosshairAnimator.SetBool("inAim", false);

        vcamAnimator.SetBool("hasScope", hasScope);
        vcamAnimator.SetBool("inAim", false);

        animator.SetBool("hasScope", hasScope);
        animator.SetBool("inAim", false);
    }

    public override void Execute(IGameEvent gEvent, bool value)
    {
        //MyDebug.Instance.Log(gEvent.GameEventName + " " + value);
        animator.SetBool("inAim", value);
        vcamAnimator.SetBool("inAim", value);
        crosshairAnimator.SetBool("inAim", value);
    }

    private void OnDisable()
    {
        RemoveGameEventFromObserver(aimEvent);
    }

    private void OnDestroy()
    {
        RemoveGameEventFromObserver(aimEvent);
    }
}
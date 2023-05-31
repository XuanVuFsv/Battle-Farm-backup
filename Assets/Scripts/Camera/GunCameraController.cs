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

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        AddGameEventToObserver(aimEvent);
    }

    public void SetHoldWeaponAnimation(bool isHoldWeapon)
    {
        //Debug.Log("Notified " + gEvent.GameEventName + " event to GunCameraController " + value);
        this.isHoldWeapon = isHoldWeapon;
        animator?.SetBool("isHoldWeapon", isHoldWeapon);

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

    public void SetHasScope(bool hasScope)
    {
        this.hasScope = hasScope;
        crosshairAnimator.SetBool("hasScope", hasScope);
        vcamAnimator.SetBool("hasScope", hasScope);
        animator.SetBool("hasScope", hasScope);
    }

    public override void Execute(IGameEvent gEvent, bool value)
    {
        animator.SetTrigger("Aim");
        vcamAnimator.SetTrigger("Aim");
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

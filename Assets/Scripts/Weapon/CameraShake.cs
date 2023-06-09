using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : GameObserver
{
    public CinemachineVirtualCamera playerCamera;
    public CinemachinePOV playerAiming;  
    public CinemachineImpulseSource cameraShake, gunCameraShake;
    public Animator rigController;
    public List<Vector2> recoilPattern;
    public Transform gunCamera;

    [SerializeField]
    GameEvent aimEvent, pickAmmoEvent;
    [SerializeField]
    InputController inputController;
    [SerializeField]
    WeaponPickup weaponPickup;
    [Tooltip("Help developer define different recoil value for different weapon with the simple value (Suggestion: Use int value)")]
    [SerializeField]
    float multiplierRecoil = 1;
    [Tooltip("To define distance's magnitude from crosshair to real postion of bullet (affected by recoil)")]
    [SerializeField]
    float scaleRecoil = 0.04f;
    [SerializeField]
    float multiplierRecoilOnAim = 1;

    public float duration;
    public float yAxisValue;

    [SerializeField]
    public int index = 0;
    [SerializeField]
    float horizontalRecoil, verticalRecoil;
    [SerializeField]
    float time;

    public void Awake()
    {
        //cameraShake = GetComponent<CinemachineImpulseSource>();
        playerAiming = playerCamera.GetCinemachineComponent<CinemachinePOV>();
        weaponPickup = GetComponent<WeaponPickup>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRecoilValue();
    }

    void UpdateRecoilValue()
    {
        yAxisValue = playerAiming.m_VerticalAxis.Value;

        if (time > 0)
        {
            playerAiming.m_HorizontalAxis.Value -= horizontalRecoil * Time.deltaTime / duration * multiplierRecoil;
            playerAiming.m_VerticalAxis.Value -= verticalRecoil * Time.deltaTime / duration * multiplierRecoil;
            time -= Time.deltaTime;
        }
    }

    public Vector3 GetCurrentPatternVector()
    {
        return new Vector3(-horizontalRecoil * multiplierRecoil * scaleRecoil, verticalRecoil * multiplierRecoil * scaleRecoil, 0);
    }

    int NextIndex()
    {
        return ((index + 1) % recoilPattern.Count);
    }
        
    public void ResetRecoil()
    {
        index = 0;
    }

    public void GenerateRecoil(AmmoStats.ZoomType zoomType)
    {
        time = duration;

        cameraShake.GenerateImpulse(Camera.main.transform.forward);
        gunCameraShake.GenerateImpulse(Camera.main.transform.forward);
        rigController.Play("Recoil");
        //Debug.Log(transform.forward);

        //random index use for sniper when scope turn off
        if (zoomType == AmmoStats.ZoomType.HasScope)
        {
            index = Random.Range(0, recoilPattern.Count);
            horizontalRecoil = recoilPattern[index].x;
            verticalRecoil = recoilPattern[index].y;
            return;
        }

        horizontalRecoil = recoilPattern[index].x;
        verticalRecoil = recoilPattern[index].y;

        index = NextIndex();
    }

    public void AssignRecoilPattern(List<Vector2> _recoilPattern)
    {
        recoilPattern = _recoilPattern;
    }

    #region Define method for Observer pattern
    public override void Execute(IGameEvent gameEvent, int value)
    {
        //Debug.Log($"Run {gameEvent} from children {this}");
        cameraShake.m_ImpulseDefinition.m_AmplitudeGain = value;
    }

    public override void Execute(IGameEvent gameEvent, float value)
    {
        //Debug.Log($"Run {gameEvent} from children {this}");
        multiplierRecoilOnAim = value;
    }

    public override void Execute(IGameEvent gameEvent, bool inAim)
    {
        //Debug.Log($"Run {gameEvent} from children {this}");
        if (inAim)
        {
            multiplierRecoil = multiplierRecoilOnAim;
        }
        else multiplierRecoil = 1;
    }

    private void OnEnable()
    {
        aimEvent.Subscribe(this);
        pickAmmoEvent.Subscribe(this);
    }

    private void OnDisable()
    {
        aimEvent.UnSubscribe(this);
        pickAmmoEvent.UnSubscribe(this);
    }
    #endregion

    public void SetUpWeaponRecoilForNewWeapon(CinemachineVirtualCamera newCamera, Animator newRigController)
    {
        playerCamera = newCamera;
        rigController = newRigController;
    }
}

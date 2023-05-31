using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo", menuName = "Ammo")]
public class AmmoStats : ScriptableObject
{
    public ActiveWeapon.WeaponSlot weaponSlot;
    public enum ZoomType
    {
        NoZoom = 0,
        CanZoom = 1,
        HasScope = 2
    }
    public ZoomType zoomType;
    public new string name;
    public int ammoAllowedInMagazine;
    public int damageHead, damageBody, damageArmsLegs;
    public int dropOffDsitance;
    public int decreseDamageRate;
    public int range;
    public int force;
    public int amplitudeGainImpulse;
    public float fireRate;
    public float runSpeed;
    public float reloadSpeed;
    public float multiplierRecoilOnAim;
    public List<Vector2> recoildPattern;
    public Sprite artwork;
    public TrailRenderer bulletTracer;
    public GameObject bulletObject;
    public ParticleSystem hitEffectPrefab;
    //public ShootingMechanic shootingMechanic;

    public enum ShootingHandleType
    {
        Raycast = 0,
        InstantiateBullet = 1
    }

    public ShootingHandleType shootingHandleType;
    public int bulletCount = 1;
    public List<Vector3> bulletDirectionPattern;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo", menuName = "Ammo")]
public class AmmoStats : ScriptableObject
{
    public enum FruitType
    {
        Berry = 0,
        Chilli = 1,
        Onion = 2,
        Punch = 4,
        Star = 5,
        Tomato = 6,
        Waternelon = 7
    }
    public FruitType fruitType;

    public enum ZoomType
    {
        NoZoom = 0,
        CanZoom = 1,
        HasScope = 2
    }
    public ZoomType zoomType;
    
    public enum BulletEffectComponent
    {
        PlaceHole = 0,
        InstantiateTrail = 1,
        Both = 2
    }
    public BulletEffectComponent bulletEffectComponent;

    public ActiveWeapon.WeaponSlot weaponSlot;
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
    public float multiplierForAmmo;
    public List<Vector2> recoildPattern;
    public Sprite artwork;
    public TrailRenderer bulletTracer;
    public GameObject bulletObject;
    public ParticleSystem hitEffectPrefab;

    public enum ShootingHandleType
    {
        Raycast = 0,
        InstantiateBullet = 1
    }

    public ShootingHandleType shootingHandleType;
    public int bulletCount = 1;
    public List<Vector3> bulletDirectionPattern;
}

using System.Collections;
using UnityEngine;
using Cinemachine;

public class BouncyThrower : MonoBehaviour
{
    private InputController inputController;

    [Header("Scene References")]
    //[SerializeField]
    //private Animator Animator;
    [SerializeField]
    private Transform Camera;
    [SerializeField]
    private Rigidbody Grenade, InUseGrenade;
    [SerializeField]
    private LineRenderer LineRenderer;
    [SerializeField]
    private Transform ReleasePosition;

    [Header("Grenade Controls")]
    [SerializeField]
    //[Range(1, 100)]
    private float ThrowStrength = 0f;
    [SerializeField]
    private float MinThrowStrength;
    [SerializeField]
    private float MaxThrowStrength;
    [SerializeField]
    private float ReachMaxThrowStrengthTime = 2f;
    [SerializeField]
    [Range(1, 10)]
    private float ExplosionDelay = 5f;
    [SerializeField]
    private GameObject ExplosionParticleSystem;

    [Header("Display Controls")]
    [SerializeField]
    //[Range(10, 100)]
    private int LinePoints = 25;
    [SerializeField]
    //[Range(0.01f, 0.25f)]
    private float TimeBetweenPoints = 0.1f;

    [Header("Display Controls")]
    public CinemachineCameraOffset throwerCamera;

    private Transform InitialParent;
    private Vector3 InitialLocalPosition;
    private Quaternion InitialRotation;

    private bool IsGrenadeThrowAvailable = true;
    private LayerMask GrenadeCollisionMask;

    private float t = 0f;

    private void Awake()
    {
        ThrowStrength = MinThrowStrength;
        InitialParent = Grenade.transform.parent;
        InitialRotation = Grenade.transform.localRotation;
        InitialLocalPosition = Grenade.transform.localPosition;
        Grenade.freezeRotation = true;

        int grenadeLayer = Grenade.gameObject.layer;
        for (int i = 0; i < 32; i++)
        {
            if (!Physics.GetIgnoreLayerCollision(grenadeLayer, i))
            {
                GrenadeCollisionMask |= 1 << i; // magic
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            //Animator.transform.rotation = Quaternion.Euler(
            //    Animator.transform.eulerAngles.x,
            //    Camera.transform.rotation.eulerAngles.y,
            //    Animator.transform.eulerAngles.z
            //);
            t += Time.deltaTime;
            ThrowStrength = Mathf.Lerp(MinThrowStrength, MaxThrowStrength, t/ReachMaxThrowStrengthTime);
            //throwerCamera.m_Offset.z = Mathf.Lerp(-5, 5, t / ReachMaxThrowStrengthTime);

            DrawProjection();
        }
        else
        {
            LineRenderer.enabled = false;
        }

        if ((Input.GetKeyUp(KeyCode.Q) || t >= ReachMaxThrowStrengthTime) && IsGrenadeThrowAvailable)
        {
            IsGrenadeThrowAvailable = false;
            t = 0f;
            //throwerCamera.m_Offset.z = -5;
            ReleaseGrenade();
            Debug.Log("Throw");
            //Animator.SetTrigger("Throw Grenade");
        }
    }

    private void DrawProjection()
    {
        IsGrenadeThrowAvailable = true;
        LineRenderer.enabled = true;
        LineRenderer.positionCount = Mathf.CeilToInt(LinePoints / TimeBetweenPoints) + 1;
        Vector3 startPosition = ReleasePosition.position;
        Vector3 startVelocity = ThrowStrength * Camera.transform.forward / Grenade.mass;
        int i = 0;
        LineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < LinePoints; time += TimeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            LineRenderer.SetPosition(i, point);

            Vector3 lastPosition = LineRenderer.GetPosition(i - 1);

            if (Physics.Raycast(lastPosition,
                (point - lastPosition).normalized,
                out RaycastHit hit,
                (point - lastPosition).magnitude,
                GrenadeCollisionMask))
            {
                LineRenderer.SetPosition(i, hit.point);
                LineRenderer.positionCount = i + 1;
                return;
            }
        }
    }

    private void ReleaseGrenade()
    {
        InUseGrenade = Instantiate(Grenade, ReleasePosition.transform.position, Quaternion.identity, Grenade.transform.parent);
        InUseGrenade.velocity = Vector3.zero;
        InUseGrenade.angularVelocity = Vector3.zero;
        InUseGrenade.isKinematic = false;
        InUseGrenade.freezeRotation = false;
        InUseGrenade.transform.SetParent(null, true);
        InUseGrenade.AddForce(ReleasePosition.forward * ThrowStrength, ForceMode.Impulse);
        StartCoroutine(ExplodeGrenade());
    }

    private IEnumerator ExplodeGrenade()
    {
        yield return new WaitForSeconds(ExplosionDelay);

        Instantiate(ExplosionParticleSystem, InUseGrenade.transform.position, Quaternion.identity);

        //Grenade.GetComponent<Cinemachine.CinemachineImpulseSource>().GenerateImpulse(new Vector3(Random.Range(-1, 1), Random.Range(0.5f, 1), Random.Range(-1, 1)));

        //Grenade.freezeRotation = true;
        //Grenade.isKinematic = true;
        //Grenade.transform.SetParent(InitialParent, false);
        //Grenade.rotation = InitialRotation;
        //Grenade.transform.localPosition = InitialLocalPosition;
        Destroy(InUseGrenade.gameObject, 1f);
        IsGrenadeThrowAvailable = true;
    }
}
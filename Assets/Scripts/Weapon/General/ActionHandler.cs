using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour, IHandGunWeaponStragety
{
    public ShootingInputData shootingInputData;
    public GrapplingRope grapplingRope;
    RaycastHit hit;

    public LineRenderer  lineRenderer;

    [SerializeField]
    private SpringJoint joint;  
    [SerializeField]
    private Vector3 grapplePoint;
    [SerializeField]
    private float maxDistance = 100f;
    [SerializeField]
    public float overshootYAxis;
    [SerializeField]
    public bool activeGrapple;

    private Rigidbody rb;
    private Vector3 velocityToSet;

    // Start is called before the first frame update
    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
    }

    //Called after Update
    //void LateUpdate()
    //{
    //    //DrawRope();
    //}

    public void SetInputData(object _inputData)
    {
        shootingInputData = _inputData as ShootingInputData;
        rb = shootingInputData.shootController.GetComponent<Rigidbody>();
        //grapplingRope.gameObject.SetActive(true);
    }

    public ShootingInputData GetShootingInputData()
    {
        return shootingInputData;
    }

    public bool HasShootingInputData()
    {
        return (shootingInputData != null && shootingInputData.bulletSpawnPoint != null);
    }

    public void HandleLeftMouseClick()
    {
        //Debug.Log("HandleLeftMouseClick");
        StartGrapple();
    }

    void StartGrapple()
    {
        if (Physics.Raycast(shootingInputData.raycastOrigin.position, shootingInputData.fpsCameraTransform.forward, out hit, maxDistance, shootingInputData.layerMask))
        {
            activeGrapple = true;
            grapplePoint = hit.point;
            if (joint == null)
            {
                SpringJoint _joint = shootingInputData.shootController.GetComponent<SpringJoint>();
                if (_joint == null)
                {
                    joint = shootingInputData.shootController.gameObject.AddComponent<SpringJoint>();
                }
                else
                {
                    joint = _joint;
                }
            }
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromTargetPoint = Vector3.Distance(shootingInputData.bulletSpawnPoint.transform.position, grapplePoint);

            //The distance grapple will try to keep from grapple point
            joint.maxDistance = distanceFromTargetPoint * 0.5f;
            joint.minDistance = distanceFromTargetPoint * 0.25f;

            joint.spring = 15f;
            joint.damper = 10f;
            joint.massScale = 1f;

            //lineRenderer.positionCount = 2;
            JumpToGrabblePoint();
        }
    }

    void JumpToGrabblePoint()
    {
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        //activeGrapple = true;

        velocityToSet = CalculateJumpVelocity(transform.position, grapplePoint, highestPointOnArc);

        Invoke(nameof(SetVelocity), 0.1f);
    }

    private void SetVelocity()
    {
        rb.velocity = velocityToSet;
    }

    public void ResetRestrictions()
    {
        activeGrapple = false;
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    void StopGrapple()
    {
        //lineRenderer.positionCount = 0;
        ResetRestrictions();
        Destroy(joint);
    }

    private Vector3 currentGrapplePosition;

    void DrawRope()
    {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lineRenderer.SetPosition(0, shootingInputData.bulletSpawnPoint.position);
        lineRenderer.SetPosition(1, currentGrapplePosition);
    }

    public void HandleRightMouseClick()
    {
        StopGrapple();
        //Debug.Log("HandleRightMouseClick");
    }

    public bool IsActiveGrapple()
    {
        return activeGrapple;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;

    }

    //public void SetInputData(object _inputData)
    //{

    //}
}

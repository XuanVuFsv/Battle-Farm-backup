using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour, IHandGunWeaponStragety
{
    public ShootingInputData shootingInputData;
    RaycastHit hit;

    public LineRenderer  lineRenderer  ;

    [SerializeField]
    private SpringJoint joint;  
    [SerializeField]
    private Vector3 targetPoint;
    [SerializeField]
    private float maxDistance = 100f;

    // Start is called before the first frame update
    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
    }

    public void SetInputData(object _inputData)
    {
        shootingInputData = _inputData as ShootingInputData;
    }

    public ShootingInputData GetShootingInputData()
    {
        return shootingInputData;
    }

    public void HandleLeftMouseClick()
    {
        Debug.Log("HandleLeftMouseClick");

        if (Physics.Raycast(shootingInputData.raycastOrigin.position, shootingInputData.fpsCameraTransform.forward, out hit, maxDistance, shootingInputData.layerMask))
        {
            targetPoint = hit.point;
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
            joint.connectedAnchor = targetPoint;

            float distanceFromTargetPoint = Vector3.Distance(shootingInputData.shootController.gameObject.transform.position, targetPoint);

            //The distance grapple will try to keep from grapple point
            joint.maxDistance = distanceFromTargetPoint * 0.8f;
            joint.minDistance = distanceFromTargetPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lineRenderer.positionCount = 2;
        }

        DrawRope();
    }

    void StopGrable()
    {
        lineRenderer.positionCount = 0;
        //Destroy(joint);
    }

    void DrawRope()
    {
        if (!joint) return;

        lineRenderer.SetPosition(0, shootingInputData.bulletSpawnPoint.position);
        lineRenderer.SetPosition(0, targetPoint);
    }

    public void HandleRightMouseClick()
    {
        Debug.Log("HandleRightMouseClick");
    }

    //public void SetInputData(object _inputData)
    //{

    //}
}

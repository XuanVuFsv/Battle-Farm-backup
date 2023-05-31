using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDetect : MonoBehaviour
{
    public Collider[] ammoDetectedList;

    [SerializeField] float detectedRangeRadius = 0;
    int layerMask;
    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Only Player");
    }

    // Update is called once per frame
    void Update()
    {
        DetectAmmo();
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.CompareTag("Player") && noParent)
        //{

        //}
    }

    void DetectAmmo()
    {
        ammoDetectedList = Physics.OverlapSphere(transform.position, detectedRangeRadius, layerMask, QueryTriggerInteraction.UseGlobal);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, detectedRangeRadius);
        Gizmos.color = Color.red;
    }
}

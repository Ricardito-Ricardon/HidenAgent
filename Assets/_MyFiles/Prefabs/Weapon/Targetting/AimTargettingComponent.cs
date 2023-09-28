using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTargettingComponent : MonoBehaviour
{
    [SerializeField] Transform aimTransform;
    [SerializeField] float aimDistance;
    [SerializeField] LayerMask aimLayerMask;

    public GameObject GetTarget()
    {
        if(Physics.Raycast(aimTransform.position, aimTransform.forward, out RaycastHit hitInfo, aimDistance, aimLayerMask))
        {
            return hitInfo.collider.gameObject;
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(aimTransform.position, aimTransform.position + aimTransform.forward * aimDistance);
    }

    Vector3 GetAimDir()
    {
        return new Vector3(aimTransform.forward.x, 0f, aimTransform.forward.z);
    }
}

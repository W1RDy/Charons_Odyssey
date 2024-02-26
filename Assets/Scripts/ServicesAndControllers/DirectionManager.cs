using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DirectionManager 
{
    public static bool IsTransformMoveToTarget(Transform transform, Vector3 moveDirection, Transform target)
    {
        return IsTransformMoveToTarget(transform, moveDirection, target.position);
    }

    public static bool IsTransformMoveToTarget(Transform transform, Vector3 moveDirection, Vector3 target)
    {
        var targetInTransformCoord = transform.InverseTransformPoint(target);
        if (Vector3.Distance(targetInTransformCoord, Vector3.zero) > Vector3.Distance(targetInTransformCoord, moveDirection))
            return true;
        return false;
    }
}

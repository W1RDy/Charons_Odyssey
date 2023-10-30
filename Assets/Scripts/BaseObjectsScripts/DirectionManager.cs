using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DirectionManager 
{
    public static bool IsTransformMoveToTarget(Transform _transform, Vector3 _moveDirection, Transform _target)
    {
        return IsTransformMoveToTarget(_transform, _moveDirection, _target.position);
    }

    public static bool IsTransformMoveToTarget(Transform _transform, Vector3 _moveDirection, Vector3 _target)
    {
        var _targetInTransformCoord = _transform.InverseTransformPoint(_target);
        if (Vector3.Distance(_targetInTransformCoord, Vector3.zero) > Vector3.Distance(_targetInTransformCoord, _moveDirection))
            return true;
        return false;
    }
}

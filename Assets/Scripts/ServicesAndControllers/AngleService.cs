using System.Collections;
using UnityEngine;

public static class AngleService
{
    public static Quaternion GetAngleByTarget(Transform transform, Transform target)
    {
        Vector3 relative = target.InverseTransformPoint(transform.position);
        return Quaternion.AngleAxis(Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg - 90 + target.rotation.eulerAngles.z, Vector3.forward);
    }
}

using UnityEngine;
using System.Collections.Generic;

public static class FinderHittableObjects
{
    public static List<IHittable> FindHittableObjectByRay(float _distance, Vector3 _weaponPosition)
    {
        var _target = (CustomCamera.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition) - _weaponPosition).normalized;
        var _hit = Physics2D.Raycast(_weaponPosition, _target, _distance, 1 << 6);
        if (_hit.collider != null) return new List<IHittable>() { _hit.collider.GetComponent<IHittable>() };
        return null;
    }

    public static List<IHittable> FindHittableObjectByCircle(float _radius, Vector2 _circlePosition)
    {
        Collider2D[] _colliders = Physics2D.OverlapCircleAll(_circlePosition, _radius, 1 << 6);
        List<IHittable> _result = new List<IHittable>();
        if (_colliders != null)
        {
            foreach (var _collider in _colliders)
            {
                var hittable = _collider.GetComponent<IHittable>();
                if (hittable != null) _result.Add(hittable);
            }
            if (_result.Count > 0) return _result;
        }
        return null;
    }
}

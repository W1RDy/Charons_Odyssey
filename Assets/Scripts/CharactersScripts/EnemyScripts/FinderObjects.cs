using UnityEngine;
using System.Collections.Generic;

public static class FinderObjects
{
    //public static List<IHittable> FindHittableObjectByRay(float _distance, Vector3 _weaponPosition, AttackableObjectIndex attackableIndex)
    //{
    //    var hittableObjLayer = attackableIndex == AttackableObjectIndex.Player ? (int)AttackableObjectIndex.Enemy : (int)AttackableObjectIndex.Player;
    //    var _target = (CustomCamera.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition) - _weaponPosition).normalized;
    //    var _hit = Physics2D.Raycast(_weaponPosition, _target, _distance, 1 << hittableObjLayer);
    //    if (_hit.collider != null) return new List<IHittable>() { _hit.collider.GetComponent<IHittable>() };
    //    return null;
    //}

    public static List<IHittable> FindHittableObjectByCircle(float radius, Vector2 circlePosition, AttackableObjectIndex attackableIndex)
    {
        var hittableObjLayer = attackableIndex == AttackableObjectIndex.Player ? (int)AttackableObjectIndex.Enemy : (int)AttackableObjectIndex.Player;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(circlePosition, radius, 1 << hittableObjLayer);
        List<IHittable> result = new List<IHittable>();
        if (colliders != null)
        {
            foreach (var collider in colliders)
            {
                var hittable = collider.GetComponent<IHittable>();
                if (hittable != null) result.Add(hittable);
            }
            if (result.Count > 0) return result;
        }
        return null;
    }

    public static IInteractable FindInteractableObjectByCircle(float radius, Vector2 circlePosition)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(circlePosition, radius, 1 << 0);
        List<IInteractable> result = new List<IInteractable>();
        if (colliders != null)
        {
            foreach (var collider in colliders)
            {
                var interactable = collider.GetComponent<IInteractable>();
                if (interactable != null) result.Add(interactable);
            }
            if (result.Count > 0) return result[0];
        }
        return null;
    }
}


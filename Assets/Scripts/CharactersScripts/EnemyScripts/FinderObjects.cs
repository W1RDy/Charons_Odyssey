using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

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
                if (collider.TryGetComponent<IHittable>(out var hittable)) result.Add(hittable);
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
                if (collider.TryGetComponent<IInteractable>(out var interactable)) result.Add(interactable);
            }
            if (result.Count > 0) return result[0];
        }
        return null;
    }

    public static Vector2 FindClosestGroundSurface(Vector2 position)
    {
        var groundCollider = FindClosestGround(position);
        return FindGroundSurface(position, groundCollider);
    }

    public static Vector2 FindGroundSurfaceInDirection(Vector2 position, Vector2 direction)
    {
        if (direction == Vector2.zero || direction.x != 0) throw new System.InvalidCastException("Invalid direction!"); 
        var groundCollider = FindGroundInDirection(position, direction);
        return FindGroundSurface(position, groundCollider);
    }

    private static Vector2 FindGroundSurface(Vector2 position, Collider2D ground)
    {
        return new Vector2(position.x, ground.bounds.max.y);
    }

    private static Collider2D FindClosestGround(Vector2 position)
    {
        var groundInDirectionUp = FindGroundInDirection(position, Vector2.up);
        var groundInDirectionDown = FindGroundInDirection(position, Vector2.down);
        if (Vector2.Distance(position, FindGroundSurface(position, groundInDirectionUp)) < Vector2.Distance(position, FindGroundSurface(position, groundInDirectionDown)))
            return groundInDirectionUp; 
        return groundInDirectionDown;
    }

    private static Collider2D FindGroundInDirection(Vector2 position, Vector2 direction)
    {
        var raycastHit = Physics2D.Raycast(position, direction, 100, 1 << 3);
        if (raycastHit.collider != null && raycastHit.collider.CompareTag("Ground"))
        {
            return raycastHit.collider;
        }
        throw new System.NullReferenceException("Ground hasn't been found!");
    }
}


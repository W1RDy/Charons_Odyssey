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
        return FindByCircle<IHittable>(radius, circlePosition, 1 << hittableObjLayer);
    }

    public static IInteractable FindInteractableObjectByCircle(float radius, Vector2 circlePosition)
    {
        var result = FindByCircle<IInteractable>(radius, circlePosition, 1 << 0 | 1 << 13);
        if (result != null) return result[0];
        return null;
    }

    public static IParryingHittable FindParryingHittableByCircle(float radius, Vector2 circlePosition)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(circlePosition, radius, 1 << (int)AttackableObjectIndex.Enemy | 1 << (int)AttackableObjectIndex.EnemyBullet);
        if (colliders != null)
        {
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<IParryingHittable>(out var hittable))
                {
                    if (hittable.IsReadyForParrying) return hittable;
                }
            }
        }
        return null;
    }

    private static List<T> FindByCircle<T>(float radius, Vector2 circlePosition, int layer)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(circlePosition, radius, layer);
        List<T> result = new List<T>();
        if (colliders != null)
        {
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<T>(out var neededObj)) result.Add(neededObj);
            }
            if (result.Count > 0) return result;
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


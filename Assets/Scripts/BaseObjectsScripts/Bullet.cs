using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DirectionalMove))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    private float _damage;
    private int _layer;

    public void Initialize(AttackableObjectIndex attackableObjectIndex, float distance, float damage)
    {
        var attackedObj = attackableObjectIndex == AttackableObjectIndex.Enemy ? AttackableObjectIndex.Player : AttackableObjectIndex.Enemy;
        _layer = (int)attackedObj;
        GetComponent<DirectionalMove>().SetSpeed(_speed);
        _damage = damage;
        Debug.Log(distance / _speed);
        Destroy(gameObject, distance / _speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_layer == collision.gameObject.layer)
        {
            collision.gameObject.GetComponent<IHittable>().TakeHit(_damage);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Ladder : MonoBehaviour
{
    [SerializeField] private PassageToFloor[] _passages;
    private Collider2D _collider;
    private bool _isUsing;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    public bool IsHaveLadderOnHeight(float height)
    {
        return _collider.bounds.max.y > height && _collider.bounds.min.y < height;
    }

    public bool IsColliderOnLaddersCenter(Collider2D collider)
    {
        return collider.bounds.min.x > _collider.bounds.min.x && collider.bounds.max.x < _collider.bounds.max.x;
    }

    public void UseLadder()
    {
        _isUsing = true;
        foreach (var ground in  _passages) ground.DeactivatePassage();
    }

    public void ThrowLadder()
    {
        _isUsing = false;
        foreach (var ground in _passages) ground.ActivatePassage();
    }

    public bool LadderIsUsing() => _isUsing;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Ladder : MonoBehaviour
{
    [SerializeField] private PassageToFloor[] _passages;
    [SerializeField] private LadderPart _ladderPartPrefab;
    private BoxCollider2D _collider;
    private bool _isUsing;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        InitializeLadder();
    }

    public void InitializeLadder()
    {
        var bottomSurface = FinderObjects.FindGroundSurfaceInDirection(transform.position, Vector2.down);
        var topSurface = FinderObjects.FindGroundSurfaceInDirection(transform.position, Vector2.up);

        var ladderLength = topSurface.y - bottomSurface.y;
        SpawnLadderParts(ladderLength, bottomSurface);
        SetCollider(ladderLength, topSurface, bottomSurface);
    }

    private void SpawnLadderParts(float ladderLength, Vector2 bottomSurface)
    {
        var spawnPosition = new Vector2(bottomSurface.x, bottomSurface.y);

        while (ladderLength > 0)
        {
            var ladderPart = SpawnLadderPart(spawnPosition, transform);

            ladderLength -= ladderPart.Height;

            if (ladderLength - ladderPart.Height < -ladderPart.Height / 2)
                spawnPosition = new Vector2(spawnPosition.x, spawnPosition.y + ladderPart.Height / 2);
            else
                spawnPosition = new Vector2(spawnPosition.x, spawnPosition.y + ladderPart.Height);
        }
    }

    private LadderPart SpawnLadderPart(Vector2 spawnPosition, Transform parent)
    {
        var ladderPart = Instantiate(_ladderPartPrefab, spawnPosition, Quaternion.identity, parent);
        return ladderPart;
    }

    private void SetCollider(float ladderLength, Vector2 topSurface, Vector2 bottomSurface)
    {
        var center = new Vector2(topSurface.x, (topSurface.y + bottomSurface.y) / 2);
        _collider.offset = transform.InverseTransformPoint(center);
        _collider.size = new Vector2(_collider.bounds.size.x, ladderLength);
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
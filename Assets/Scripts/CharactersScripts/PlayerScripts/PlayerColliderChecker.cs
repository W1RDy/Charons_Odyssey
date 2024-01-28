using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerColliderChecker : MonoBehaviour
{
    private HashSet<Collider2D> _grounds;
    private Collider2D _ladder;

    private void Awake()
    {
        _grounds = new HashSet<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && collision.GetContact(collision.contacts.Length - 1).point.y > collision.collider.bounds.center.y)
        {
            _grounds.Add(collision.collider);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            RemoveGround(collision.collider);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ladder")
        {
            _ladder = collision.GetComponent<Collider2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ladder")
        {
            _ladder = null;
        }
    }

    public void RemoveGround(Collider2D collider)
    {
        if (_grounds.Contains(collider)) _grounds.Remove(collider);
    }

    public bool IsCollideWithGround()
    {
        return _grounds.Count > 0;
    }

    public bool IsCollideWithLadder()
    {
        return _ladder != null;
    }

    public bool TryGetLadder(out Ladder ladder)
    {
        ladder = null;
        var isCollideWithLadder = IsCollideWithLadder();
        if (isCollideWithLadder) ladder = _ladder.GetComponent<Ladder>();
        return isCollideWithLadder;
    }
}

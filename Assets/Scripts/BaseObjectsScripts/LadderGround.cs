using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class LadderGround : MonoBehaviour
{
    private Collider2D _collider;
    private PlayerMove _player;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player") DisableGround();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") EnableGround();
    }

    private void DisableGround()
    {
        if (DirectionManager.IsTransformMoveToTarget(_player.transform, new Vector2(0,_player.GetDirection().y), transform))
            _collider.isTrigger = true;
    }

    private void EnableGround()
    {
        _collider.isTrigger = false;
    }
}

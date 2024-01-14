using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PassageToFloor : MonoBehaviour
{
    private Collider2D _collider;
    private Player _player;

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.ClosestPoint(_player.transform.position).y > transform.position.y)
        {
            ActivatePassage();
        }
    }

    public void DeactivatePassage()
    {
        _collider.isTrigger = true;
        _player.RemoveGround(_collider);
    }

    public void ActivatePassage()
    {
        _collider.isTrigger = false;
    }
}

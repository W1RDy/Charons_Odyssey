using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PassageToFloor : MonoBehaviour
{
    private Collider2D _collider;
    private PlayerColliderChecker _playerColliderChecker;

    [Inject]
    private void Construct(Player player)
    {
        _playerColliderChecker = player.GetComponent<PlayerColliderChecker>();
    }

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.ClosestPoint(_playerColliderChecker.transform.position).y > transform.position.y)
        {
            ActivatePassage();
        }
    }

    public void DeactivatePassage()
    {
        _collider.isTrigger = true;
        _playerColliderChecker.RemoveGround(_collider);
    }

    public void ActivatePassage()
    {
        Debug.Log("Activate");
        _collider.isTrigger = false;
    }
}

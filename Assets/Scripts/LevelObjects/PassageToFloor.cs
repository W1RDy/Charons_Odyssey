using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(BoxCollider2D))]
public class PassageToFloor : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _groundCollider;
    private PlayerColliderChecker _playerColliderChecker;

    [Inject]
    private void Construct(Player player)
    {
        _playerColliderChecker = player.GetComponent<PlayerColliderChecker>();
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
        _groundCollider.gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
        _playerColliderChecker.RemoveGround(_groundCollider);
    }

    public void ActivatePassage()
    {
        _groundCollider.gameObject.layer = LayerMask.NameToLayer("Ground");
    }
}

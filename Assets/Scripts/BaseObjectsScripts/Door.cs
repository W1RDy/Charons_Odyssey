using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer _view;
    [SerializeField] private bool _isLocked;
    private Space _space;
    private Collider2D _collider;
    private Inventory _inventory;

    [Inject]
    private void Construct(Inventory inventory)
    {
        _inventory = inventory;
    }

    private void Awake()
    {
        _space = GetComponentInParent<Space>();
        _collider = GetComponent<Collider2D>();
    }

    private void OpenDoor()
    {
        if (!_isLocked || (_isLocked && _inventory.HasItem(ItemType.Key)))
        {
            if (_isLocked)
            {
                _inventory.RemoveItem(ItemType.Key);
                _isLocked = false;
            }
            _collider.enabled = false;
            _space.OpenSpace();
            _view.gameObject.SetActive(false);
        }
    }

    private void CloseDoor()
    {

    }

    public void Interact()
    {
        OpenDoor();
    }
}

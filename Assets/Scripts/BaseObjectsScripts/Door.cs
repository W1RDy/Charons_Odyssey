using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer _view;
    [SerializeField] private bool _isLocked;
    private Space _space;
    private Collider2D _collider;

    private void Awake()
    {
        _space = GetComponentInParent<Space>();
        _collider = GetComponent<Collider2D>();
    }

    private void OpenDoor()
    {
        if (!_isLocked || (_isLocked && Inventory.Instance.HasItem(ItemType.Key)))
        {
            if (_isLocked)
            {
                Inventory.Instance.RemoveItem(ItemType.Key);
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

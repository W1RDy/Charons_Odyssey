using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer _view;
    [SerializeField] private bool _isLocked;
    [SerializeField] private Space[] _spaces;
    private Inventory _inventory;

    private Collider2D _collider;
    private bool _colliderIsTrigger;

    private AudioMaster _audioMaster;

    [SerializeField] private bool _isCanBeClosed;
    private bool _isOpened;

    [Inject]
    private void Construct(Inventory inventory, AudioMaster audioMaster)
    {
        _inventory = inventory;
        _audioMaster = audioMaster;
    }

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _colliderIsTrigger = _collider.isTrigger;
    }

    private void OpenDoor()
    {
        if (!_isLocked || (_isLocked && _inventory.HasItem(ItemType.Key)))
        {
            if (_isLocked)
            {
                _inventory.RemoveItem(ItemType.Key);
                _isLocked = false;
                _audioMaster.PlaySound("OpenIronDoor");
            }
            else
            {
                _audioMaster.PlaySound("OpenWoodDoor");
            }

            _isOpened = true;
            _collider.isTrigger = true;

            foreach (var space in _spaces) space.OpenSpace();
            if (_view) _view.gameObject.SetActive(false);
        }
    }

    private void CloseDoor()
    {
        _isOpened = false;
        _collider.isTrigger = _colliderIsTrigger;

        foreach (var space in _spaces) space.CloseSpace();
        if (_view) _view.gameObject.SetActive(true);

        _audioMaster.PlaySound("OpenWoodDoor");
    }

    public void Interact()
    {
        if (!_isOpened) OpenDoor();
        else if (_isCanBeClosed) CloseDoor();
    }
}

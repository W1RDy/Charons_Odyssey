using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class InventoryItem : Item
{
    protected Inventory _inventory;

    [Inject]
    private void Construct(Inventory inventory)
    {
        _inventory = inventory;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _inventory.AddItem(this);
            gameObject.SetActive(false);
            _collider.enabled = false;
        }
    }
}

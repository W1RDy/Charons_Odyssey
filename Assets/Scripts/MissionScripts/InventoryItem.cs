using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InventoryItem : Item
{
    private Inventory _inventory;

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

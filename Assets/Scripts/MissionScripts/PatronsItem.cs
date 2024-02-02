using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PatronsItem : InventoryItem
{
    [SerializeField] private int patronsCount;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _inventory.AddItem(this, patronsCount);
            gameObject.SetActive(false);
            _collider.enabled = false;
        }
    }
}

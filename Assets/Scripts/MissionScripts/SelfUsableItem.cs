using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class SelfUsableItem : Item
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.SetActive(false);
            _collider.enabled = false;
        }
    }
}

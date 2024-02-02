using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Item : MonoBehaviour
{
    [SerializeField] protected ItemType _type;
    protected Collider2D _collider;
    public ItemType Type { get => _type; }

    protected virtual void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }
}

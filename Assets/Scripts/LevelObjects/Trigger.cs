using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(BoxCollider2D))]
public class Trigger : MonoBehaviour
{
    public event Action TriggerWorked;
    public event Action TriggerTurnedOff;
    public bool PlayerInTrigger { get; private set; }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TriggerWorked?.Invoke();
            PlayerInTrigger = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TriggerTurnedOff?.Invoke();
            PlayerInTrigger = false;
        }
    }
}

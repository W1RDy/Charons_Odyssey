using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(BoxCollider2D))]
public class Trigger : MonoBehaviour
{
    public event Action TriggerWorked;
    public event Action TriggerTurnedOff;
    public bool playerInTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TriggerWorked?.Invoke();
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TriggerTurnedOff?.Invoke();
            playerInTrigger = false;
        }
    }
}

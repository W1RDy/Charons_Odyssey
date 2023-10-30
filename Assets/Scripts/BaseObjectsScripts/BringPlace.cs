using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BringPlace : MonoBehaviour
{
    [SerializeField] TaskBringSomething _task;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") _task.FinishTask();
    }
}

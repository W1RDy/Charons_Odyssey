using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHasHealth 
{
    [SerializeField] private float _hp;

    public void TakeHit(float damage)
    {
        _hp -= damage;
        if (_hp <= 0) Death();
    }

    public void Death()
    {
        gameObject.SetActive(false);
    }
}

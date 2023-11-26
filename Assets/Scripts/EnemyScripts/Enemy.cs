using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHasHealth, IAttackable
{
    [SerializeField] protected float _hp;
    public EnemyStates State { get; private set;}

    public void TakeHit(float damage)
    {
        _hp -= damage;
        if (_hp <= 0) Death();
    }

    public void Death()
    {
        gameObject.SetActive(false);
    }

    public virtual void Attack()
    {

    }

    public void ChangeState(EnemyStates state)
    {
        Debug.Log("state: " + state);
        State = state;
    }
}

public enum EnemyStates
{
    Idle,
    Moving,
    Reclined,
    WaitingCooldown,
    Attacking
}

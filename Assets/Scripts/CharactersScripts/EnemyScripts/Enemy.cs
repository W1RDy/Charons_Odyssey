using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHasHealth, IAttackable
{
    [SerializeField] protected float _hp;
    public EnemyStates State { get; private set;}
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;

    protected virtual void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void TakeHit(float damage)
    {
        StartCoroutine(TakeHit());
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
        if (state == EnemyStates.Idle || state == EnemyStates.WaitingCooldown || state == EnemyStates.Reclined)
        {
            _animator.SetBool("Move", false);
            _animator.SetBool("Attack", false);
        }
        else if (state == EnemyStates.Moving) _animator.SetBool("Move", true);
        else if (state == EnemyStates.Attacking) _animator.SetBool("Attack", true);

        Debug.Log("state: " + state);
        State = state;
    }

    IEnumerator TakeHit() // переделать
    {
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
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

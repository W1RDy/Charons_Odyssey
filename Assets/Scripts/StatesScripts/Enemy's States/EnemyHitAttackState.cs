using UnityEngine;

[CreateAssetMenu(fileName = "Hit Attack State", menuName = "Enemy's State/Hit Attack State")]
public class EnemyHitAttackState : EnemyAttackState
{
    protected override void Attack()
    {
        var player = FinderObjects.FindHittableObjectByCircle(_enemy.HitDistance, _enemy.transform.position, AttackableObjectIndex.Enemy);

        if (player != null)
        {
            player[0].TakeHit(new HitInfo(_enemy.Damage, new Vector2(_enemy.transform.localScale.x, 0).normalized));
        }
    }
}
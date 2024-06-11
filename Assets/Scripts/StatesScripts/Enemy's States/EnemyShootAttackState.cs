using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

[CreateAssetMenu(fileName = "Shoot Attack State", menuName = "Enemy's State/Shoot Attack State")]
public class EnemyShootAttackState : EnemyAttackState
{
    [SerializeField] private GorgonBullet _bulletPrefab;

    public override void Initialize(Enemy enemy, IInstantiator instantiator, Transform target)
    {
        base.Initialize(enemy, instantiator, target);
    }

    public override void Update()
    {
        var vectorToTarget = (_target.transform.position - _enemy.transform.position);
        _enemy.Flip(vectorToTarget.normalized);
    }

    protected override void Attack()
    {
        var bulletObj = SpawnBullet();
    }

    private Bullet SpawnBullet()
    {
        Bullet bulletObj;
        if (_enemy is Gorgon gorgon)
            bulletObj = Instantiate(_bulletPrefab, gorgon.ShootPoint.position, Quaternion.identity);
        else
            bulletObj = Instantiate(_bulletPrefab, _enemy.transform.position, Quaternion.identity);

        var direction = (_target.transform.position - bulletObj.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (_enemy.transform.localScale.x < 0) bulletObj.transform.localScale = new Vector3(bulletObj.transform.localScale.x, -bulletObj.transform.localScale.y, 1);

        bulletObj.transform.eulerAngles = new Vector3(0, 0, angle);

        bulletObj.Initialize(_instantiator, _enemy.HitDistance, _enemy.Damage);

        return bulletObj;
    }
}
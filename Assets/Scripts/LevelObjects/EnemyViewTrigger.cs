using UnityEngine;

public class EnemyViewTrigger : Trigger
{
    private Transform _enemy;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>().transform;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (IsSeePlayer(collision.transform)) base.OnTriggerEnter2D(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !PlayerInTrigger)
        {
            if (IsSeePlayer(collision.transform)) base.OnTriggerEnter2D(collision);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            base.OnTriggerExit2D(collision);
        }
    }

    private bool IsSeePlayer(Transform player)
    {
        Physics2D.queriesHitTriggers = false;
        var vectorBetween = player.position - _enemy.position;
        var hit = Physics2D.Raycast(_enemy.position, vectorBetween.normalized, vectorBetween.magnitude, 1 << 3 | 1 << 7 | 1 << 13);
        Physics2D.queriesHitTriggers = true;

        return hit.collider != null && hit.collider.CompareTag("Player");
    }
}

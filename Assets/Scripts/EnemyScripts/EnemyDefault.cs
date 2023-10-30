using System.Collections;
using UnityEngine;

public class EnemyDefault : Enemy, IReclinable
{
    Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void GetRecline(Transform _recliner, float _reclineForce)
    {
        _rb.AddForce((transform.position - _recliner.position).normalized * _reclineForce, ForceMode2D.Impulse);
    }
}

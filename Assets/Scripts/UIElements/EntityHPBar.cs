using UnityEngine;
using UnityEngine.UI;

public class EntityHPBar : MonoBehaviour, IHPBar
{
    [SerializeField] private Image _hpImage;

    private float _maxHP;
    private Transform _entity;

    private Vector2 _offset;

    public void Init(float maxHP, Transform entity)
    {
        _maxHP = maxHP;
        _entity = entity;

        _offset = transform.position - entity.position;
    }

    private void Update()
    {
        if (transform.position != _entity.position.SumWithoutZCoordinate(_offset))
        {
            transform.position = _entity.position.SumWithoutZCoordinate(_offset);
        }
    }

    public void SetHP(float hp)
    {
        if (hp <= 0) gameObject.SetActive(false);
        else
        {
            float hpAmount = (float)hp / _maxHP;
            _hpImage.fillAmount = hpAmount;
        }
    }
}
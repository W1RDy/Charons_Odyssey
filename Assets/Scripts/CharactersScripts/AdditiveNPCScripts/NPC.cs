using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class NPC : MonoBehaviour, ITalkable, IAvailable
{
    [SerializeField] private string _talkableIndex;
    [SerializeField] private Trigger _trigger;
    [SerializeField] private float _talkDelay;
    [SerializeField] private string _branchIndex;
    [SerializeField] protected bool _isAvailable = true;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _trigger.TriggerWorked += Talk;
    }

    public Vector2 GetTalkableTopPoint()
    {
        return new Vector2(transform.position.x, transform.position.y + _spriteRenderer.sprite.bounds.size.y);
    }

    public void Talk()
    {
        if (_isAvailable)
        {
            DialogManager.Instance.StartDialog(_branchIndex, this, _talkDelay);
            _trigger.TriggerWorked -= Talk;
        }
    }

    public string GetTalkableIndex()
    {
        return _talkableIndex;
    }

    public void ChangeAvailable(bool isAvailable)
    {
        _isAvailable = isAvailable;
    }
}

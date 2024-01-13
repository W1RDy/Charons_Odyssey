using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class NPC : MonoBehaviour, ITalkable, IAvailable
{
    [SerializeField] private string _talkableIndex;
    [SerializeField] private Trigger _trigger;
    [SerializeField] private float _talkDelay;
    [SerializeField] private string _branchIndex;
    [SerializeField] protected bool _isAvailable = true;
    [SerializeField] protected DialogCloudService _dialogCloudService; 
    protected SpriteRenderer _spriteRenderer;

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _trigger.TriggerWorked += StartDialog;
    }

    public void Talk(string message)
    {
        _dialogCloudService.SpawnDialogCloud(new Vector2(transform.position.x, transform.position.y + _spriteRenderer.sprite.bounds.size.y));
        _dialogCloudService.UpdateDialogCloud(message);
    }

    public void StartDialog()
    {
        if (_isAvailable)
        {
            DialogManager.Instance.ActivateDialog(_branchIndex, new ITalkable[] {this});
            _trigger.TriggerWorked -= StartDialog;
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

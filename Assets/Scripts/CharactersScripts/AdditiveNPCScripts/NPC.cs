using UnityEngine;
using Zenject;

public abstract class NPC : MonoBehaviour, ITalkable, IAvailable, IPause
{
    [SerializeField] private string _talkableIndex;
    [SerializeField] private string _branchIndex;

    [SerializeField] private float _talkDelay;
    [SerializeField] protected bool _isAvailable = true;

    [SerializeField] private Trigger _trigger;

    protected SpriteRenderer _spriteRenderer;

    protected DialogCloudService _dialogCloudService; 
    private DialogActivator _dialogActivator;

    private PauseService _pauseService;

    [Inject]
    private void Consruct(DialogActivator dialogActivator, DialogCloudService dialogCloudService, PauseService pauseService)
    {
        _dialogActivator = dialogActivator;
        _dialogCloudService = dialogCloudService;
        _pauseService = pauseService;
        _pauseService.AddPauseObj(this);
    }

    public virtual void InitializeNPC(Direction direction, string dialogId, bool isAvailable)
    {
        if (direction == Direction.Left) transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        UpdateDialog(dialogId);
        ChangeAvailable(isAvailable);

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_branchIndex != "") _trigger.TriggerWorked += StartDialog;
    }

    public void Talk(string message)
    {
        if (_isAvailable)
        {
            _dialogCloudService.SpawnDialogCloud(new Vector2(transform.position.x, transform.position.y + _spriteRenderer.sprite.bounds.size.y));
            _dialogCloudService.UpdateDialogCloud(message);
        }
    }

    public void StartDialog()
    {
        if (_isAvailable)
        {
            _dialogActivator.ActivateDialog(_branchIndex);
            _trigger.TriggerWorked -= StartDialog;
        }
    }

    public void UpdateDialog(string dialogIndex)
    {
        _branchIndex = dialogIndex;
        if (_branchIndex != "") _trigger.TriggerWorked += StartDialog;
    }

    public string GetTalkableIndex()
    {
        return _talkableIndex;
    }

    public void ChangeAvailable(bool isAvailable)
    {
        _isAvailable = isAvailable;
    }

    public virtual void Pause()
    {
        if (_isAvailable == true) _isAvailable = false;
    }

    public void Unpause()
    {
        if (_isAvailable == false) _isAvailable = true;
    }

    public void OnDestroy()
    {
        _pauseService.RemovePauseObj(this);
    }
}

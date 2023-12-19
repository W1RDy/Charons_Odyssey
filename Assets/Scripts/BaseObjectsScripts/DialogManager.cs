using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private DialogBranch[] dialogBranches;
    [SerializeField] private DialogCloudService _dialogCloudService;
    private DialogBranch _currentBranch;
    private bool dialogIsFinished = true;

    public static DialogManager Instance;
    private Dictionary<string, DialogBranch> branchDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeMessagesDictionary();
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(Instance);
        Instance._dialogCloudService = GameObject.Find("DialogCloudService").GetComponent<DialogCloudService>();
    }

    private void InitializeMessagesDictionary()
    {
        branchDictionary = new Dictionary<string, DialogBranch>();

        foreach (var dialogBranch in dialogBranches) branchDictionary[dialogBranch.index] = dialogBranch;
    }

    public void StartDialog(string _branchIndex, ITalkable _talkable, float _delay)
    {
        dialogIsFinished = false;
        ActivateDialog(_branchIndex, _talkable);
        DialogLife(_talkable, _delay);
    }

    private async void DialogLife(ITalkable _talkable, float _delay)
    {
        while (!dialogIsFinished)
        {
            var token = this.GetCancellationTokenOnDestroy();
            await Delayer.Delay(_delay, token);
            if (token.IsCancellationRequested) break;
            ActivateNextMessage(_talkable);
        }
    }

    public void FinishDialog()
    {
        _dialogCloudService.RemoveDialogCloud();
        dialogIsFinished = true;
    }

    public void ChangeTalkable(string _index, ITalkableGroup _talkableGroup)
    {
        _talkableGroup.ChangeTalkable(_index);
        _dialogCloudService.SetSpawnPosition(_talkableGroup); // исправить обязательно
    }

    private void ActivateDialog(string _branchIndex, ITalkable _talkable)
    {
        _currentBranch = branchDictionary[_branchIndex];
        var _message = _currentBranch.GetFirstMessage();
        _dialogCloudService.SpawnDialogCloud(_talkable);
        _dialogCloudService.UpdateDialogCloud(_message.message);
    }

    private void ActivateNextMessage(ITalkable _talkable)
    {
        var _message = _currentBranch.GetNextMessage();
        if (_message != null) _dialogCloudService.UpdateDialogCloud(_message.message);
        else FinishDialog();

        if (_message != null && _message.isNeedChangeTalkable) ChangeTalkable(_message.talkableIndex, _talkable as ITalkableGroup);
    }

    public bool DialogIsFinished() => dialogIsFinished;
}

[Serializable]
public class DialogBranch
{
    public string index;
    public MessageConfig[] messageConfigs;
    private int _messageIndexInOrder;

    public MessageConfig GetFirstMessage()
    {
        _messageIndexInOrder = -1;
        return GetNextMessage();
    }

    public MessageConfig GetNextMessage()
    {
        _messageIndexInOrder++;
        return _messageIndexInOrder < messageConfigs.Length ? messageConfigs[_messageIndexInOrder] : null;
    }
}

[Serializable]
public class MessageConfig
{
    public string message;
    public bool isNeedChangeTalkable;
    public string talkableIndex;
}

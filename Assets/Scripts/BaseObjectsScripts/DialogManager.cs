using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DialogManager : MonoBehaviour // разбить класс
{
    private bool _dialogIsFinished = true;
    private Dictionary<string, DialogBranch> _branchDictionary;
    public event Action<string> DeactivateDialog;
    private DialogBranch _currentDialog;

    private void Awake()
    {
        InitializeMessagesDictionary();
    }

    private async void InitializeMessagesDictionary()
    {
        _branchDictionary = await DownloaderDataFromGoogleSheets.DownloadDialogsData();

        //foreach (var branch in  _branchDictionary)
        //{
        //    Debug.Log(branch.Key);
        //    foreach(var message in  branch.Value.messageConfigs)
        //    {
        //        Debug.Log(message.talkableIndex);
        //    }
        //}
    }

    public void ActivateDialog(string branchIndex, ITalkable[] talkables, CancellationToken token)
    {
        if (_currentDialog == null)
        {
            _currentDialog = _branchDictionary[branchIndex];
            _dialogIsFinished = false;
            var message = _currentDialog.GetFirstMessage();
            var talkable = ChooseTalkable(talkables, message.talkableIndex);
            talkable.Talk(message.message);
            Dialog(2f, talkables, token);
        }
    }

    public async void Dialog(float delay, ITalkable[] talkables, CancellationToken token)
    {
        while (true)
        {
            await Delayer.Delay(delay, token);
            if (token.IsCancellationRequested || _dialogIsFinished) break;
            ActivateNextMessage(talkables);
        }
    }

    private void ActivateNextMessage(ITalkable[] talkables)
    {
        var message = _currentDialog.GetNextMessage();
        if (message == null)
        {
            FinishDialog();
            return;
        }

        var talkable = ChooseTalkable(talkables, message.talkableIndex);
        talkable.Talk(message.message);
    }

    public void FinishDialog()
    {
        _dialogIsFinished = true;
        DeactivateDialog(_currentDialog.index);
        _currentDialog = null;
    }

    private ITalkable ChooseTalkable(ITalkable[] talkables, string index)
    {
        index = index.Replace(" ", "");;
        foreach (var talkable in talkables)
        {
            if (talkable.GetTalkableIndex() == index) return talkable;
        }
        throw new ArgumentNullException("Talkable with index " + index + " doesn't exist!");
    }

    public bool DialogIsFinished() => _dialogIsFinished;
}

[Serializable]
public class DialogBranch
{
    public string index;
    public List<MessageConfig> messageConfigs;
    private int _messageIndexInOrder;

    public DialogBranch(string index, List<MessageConfig> messageConfigs)
    {
        this.index = index;
        this.messageConfigs = messageConfigs;
        _messageIndexInOrder = -1;
    }

    public MessageConfig GetFirstMessage()
    {
        _messageIndexInOrder = -1;
        return GetNextMessage();
    }

    public MessageConfig GetNextMessage()
    {
        _messageIndexInOrder++;
        return _messageIndexInOrder < messageConfigs.Count ? messageConfigs[_messageIndexInOrder] : null;
    }
}

[Serializable]
public class MessageConfig
{
    public string message;
    public bool isNeedChangeTalkable;
    public string talkableIndex;

    public MessageConfig(string message, bool isNeedChangeTalkable, string talkableIndex)
    {
        this.message = message;
        this.isNeedChangeTalkable = isNeedChangeTalkable;
        this.talkableIndex = talkableIndex;
    }
}

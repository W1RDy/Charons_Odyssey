using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogService 
{
    private Dictionary<string, DialogBranch> _branchDictionary;

    public void InitializeSevice()
    {
        InitializeMessagesDictionary();
    }

    private async void InitializeMessagesDictionary()
    {
        _branchDictionary = await DownloaderDataFromGoogleSheets.DownloadDialogsData();

        foreach (var branch in _branchDictionary)
        {
            Debug.Log(branch.Key);
            foreach (var message in branch.Value.messageConfigs)
            {
                Debug.Log(message.talkableIndex);
            }
        }
    }

    public DialogBranch GetDialog(string dialogId)
    {
        if (!_branchDictionary.ContainsKey(dialogId)) throw new NullReferenceException("Dialog with index " + dialogId + " doesn't exist!");
        return _branchDictionary[dialogId];
    }
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

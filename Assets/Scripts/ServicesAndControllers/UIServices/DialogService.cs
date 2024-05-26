using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogService : IService 
{
    private Dictionary<string, DialogBranch> _branchDictionary = new Dictionary<string, DialogBranch>();

    private const string DialogFilePath = "Configs/DialogFile.json";

    public void InitializeService()
    {
        InitializeMessagesDictionary();
    }

    private void InitializeMessagesDictionary()
    {
        var json = File.ReadAllText("Assets/Resources/" + DialogFilePath);
        var dialogConfigs = JsonUtility.FromJson<DialogConfigs>(json);

        foreach (var dialogBranch in dialogConfigs.DialogBranches)
        {
            _branchDictionary.Add(dialogBranch.index, dialogBranch);
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
    public string russianMessage;
    public string talkableIndex;
}

[Serializable]
public class DialogConfigs
{
    public List<DialogBranch> DialogBranches;
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogParser : IGoogleSheetParser
{
    private readonly DialogConfigs _dialogConfigs;
    private DialogBranch _currentDialogBranch;
    private MessageConfig _currentMessageConfig;

    private bool _isSkipTitle;

    public DialogParser(DialogConfigs dialogConfigs)
    {
        _dialogConfigs = dialogConfigs;
        _dialogConfigs.DialogBranches = new List<DialogBranch>();
    }

    public void Parse(string header, string token)
    {
        if (_isSkipTitle)
        {
            _isSkipTitle = false;
            return;
        }

        Debug.Log($"Header{header}, token{token}");

        if (token.StartsWith("branch_"))
        {
            if (_currentDialogBranch != null) _dialogConfigs.DialogBranches.Add(_currentDialogBranch);

            _currentDialogBranch = new DialogBranch(token.Remove(0, 7), new List<MessageConfig>());
            _isSkipTitle = true;
            return;
        }
        else if (header == "End")
        {
            _dialogConfigs.DialogBranches.Add(_currentDialogBranch);
            return;
        }

        switch (header)
        {
            case "key":
                _currentMessageConfig = new MessageConfig() { talkableIndex = token };
                break;
            case "russian":
                _currentMessageConfig.russianMessage = token;
                _currentDialogBranch.messageConfigs.Add(_currentMessageConfig);
                break;
            default:
                throw new Exception($"Invalid header: {header}!");
        }
    }
}

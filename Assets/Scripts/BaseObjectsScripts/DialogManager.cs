using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private DialogBranch[] _dialogBranches;
    [SerializeField] private DialogCloudService _dialogCloudService;
    private bool _dialogIsFinished = true;
    private Player _charon;

    public static DialogManager Instance;
    private Dictionary<string, DialogBranch> _branchDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeMessagesDictionary();
        }

        DontDestroyOnLoad(Instance);
    }

    private void Start()
    {
        if (Instance != this) Destroy(gameObject);
        Instance._charon = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Instance._dialogCloudService = GameObject.Find("DialogCloudService").GetComponent<DialogCloudService>();
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

    public void ActivateDialog(string branchIndex, ITalkable[] talkables)
    {
        _charon.StartTalk();
        DialogBranch branch = _branchDictionary[branchIndex];
        _dialogIsFinished = false;
        var message = branch.GetFirstMessage();
        var talkable = ChooseTalkable(talkables, message.talkableIndex);
        talkable.Talk(message.message);
        Dialog(2f, branch, talkables);
    }

    public async void Dialog(float delay, DialogBranch branch, ITalkable[] talkables)
    {
        while (!_dialogIsFinished)
        {
            var token = this.GetCancellationTokenOnDestroy();
            await Delayer.Delay(delay, token);
            if (token.IsCancellationRequested) break;
            ActivateNextMessage(branch, talkables);
        }
    }

    private void ActivateNextMessage(DialogBranch branch, ITalkable[] talkables)
    {
        var message = branch.GetNextMessage();
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
        _dialogCloudService.RemoveDialogCloud();
        _dialogIsFinished = true;
    }

    private ITalkable ChooseTalkable(ITalkable[] talkables, string index)
    {
        index = index.Replace(" ", "");;
        if (index == _charon.GetTalkableIndex())
        {
            return _charon;
        }
        foreach (var talkable in talkables)
        {
            Debug.Log(talkable.GetTalkableIndex());
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

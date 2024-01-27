using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DialogUpdater : MonoBehaviour
{
    private TalkableFinderOnLevel _talkableFinder;

    [Inject]
    private void Construct(TalkableFinderOnLevel talkableFinder)
    {
        _talkableFinder = talkableFinder;
    }

    public void UpdateDialog(string npcIndex, string newDialogIndex)
    {
        var npc = _talkableFinder.GetTalkable(npcIndex) as NPC;
        if (npc == null) throw new NullReferenceException("NPC with index " + npcIndex + "doesn't exist!");
        npc.UpdateDialog(newDialogIndex);
    }
}

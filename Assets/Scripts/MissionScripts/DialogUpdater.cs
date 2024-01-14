using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogUpdater : MonoBehaviour
{
    [SerializeField] private NPC[] _npcs;

    public void UpdateDialog(string npcIndex, string newDialogIndex)
    {
        var npc = FindNpc(npcIndex);
        npc.UpdateDialog(newDialogIndex);
    }

    private NPC FindNpc(string index)
    {
        foreach (NPC npc in _npcs)
        {
            if (npc.GetTalkableIndex() == index) return npc;
        }
        throw new ArgumentNullException("NPC with index " + index + " doesn't exist!");
    }
}

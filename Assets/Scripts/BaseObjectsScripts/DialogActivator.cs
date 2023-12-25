using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class DialogActivator
{
    private ITalkable _charon;
    private bool dialogIsFinished;
    private CancellationToken _token;

    public DialogActivator(Player player, CancellationToken token)
    {
        _charon = player;
        _token = token;
    }


}

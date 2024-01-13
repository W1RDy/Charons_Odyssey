using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class Delayer
{
    public async static UniTask Delay(float delay, CancellationToken token)
    {
        await UniTask.Delay((int)(delay * 1000), cancellationToken: token).SuppressCancellationThrow();
    }
}

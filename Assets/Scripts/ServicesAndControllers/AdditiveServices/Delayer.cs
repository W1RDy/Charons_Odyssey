using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class Delayer
{
    public static async UniTask Delay(float delay, CancellationToken token)
    {
        await UniTask.Delay((int)(delay * 1000), cancellationToken: token).SuppressCancellationThrow();
    }

    public static async UniTask DelayWithPause(float delay, CancellationToken token, PauseToken pauseToken)
    {
        var startTime = Time.time;
        var remainingTime = delay;
        
        while (remainingTime > 0)
        {
            var combinatedToken = CancellationTokenSource.CreateLinkedTokenSource(pauseToken.CancellationToken, token).Token;
            await Delay(remainingTime, combinatedToken);
            remainingTime -= (Time.time - startTime);
            await UniTask.WaitWhile(() => pauseToken.IsCancellationRequested);
        }
    }
}

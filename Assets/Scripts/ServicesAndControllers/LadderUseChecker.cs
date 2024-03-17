using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LadderUseChecker
{
    public bool IsCanUse(Player player, Ladder ladder, float direction)
    {
        if (ladder == null || direction == 0 || !player.OnGround()) return false;

        var playerCollider = player.GetComponent<Collider2D>();
        var normalizedDirection = direction > 0 ? 1 : -1;
        var predictedPos = playerCollider.bounds.center.y + normalizedDirection * (playerCollider.bounds.extents.y + 0.1f);

        return ladder.IsColliderOnLaddersCenter(playerCollider) && ladder.IsHaveLadderOnHeight(predictedPos);
    }

    public bool IsCanThrow(Player player, Ladder ladder, float direction)
    {
        return ladder != null && ladder.LadderIsUsing() && player.OnGround() && direction <= 0;
    }
}

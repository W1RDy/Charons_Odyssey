using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMoveChecker
{
    private Ladder _ladder;

    public void SetLadderToCheck(Ladder ladder)
    {
        _ladder = ladder;
    }

    public void RemoveLadderToCheck()
    {
        _ladder = null;
    }

    public Ladder GetLadder()
    {
        return _ladder;
    }

    public bool IsCanUse(Player player, float direction)
    {
        if (_ladder == null || direction == 0 || !player.OnGround()) return false;

        var playerCollider = player.GetComponent<Collider2D>();
        var normalizedDirection = direction > 0 ? 1 : -1;
        var predictedPos = playerCollider.bounds.center.y + normalizedDirection * playerCollider.bounds.size.y;

        return _ladder.IsHaveLadderOnHeight(predictedPos);
    }
}

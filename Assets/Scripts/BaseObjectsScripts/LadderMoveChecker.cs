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

    public bool IsCanMove(Player player, float direction)
    {
        var onGround = player.OnGround();
        return IsCanMove(player, direction, onGround);
    }

    private bool IsCanMove(Player player, float direction, bool isStartMoving)
    {
        if (_ladder == null || direction == 0) return false;

        var playerCollider = player.GetComponent<Collider2D>();
        var normalizedDirection = direction > 0 ? 1 : -1;

        float predictedPos; 
        if (isStartMoving) predictedPos = playerCollider.bounds.center.y + normalizedDirection * playerCollider.bounds.size.y;
        else predictedPos = playerCollider.bounds.min.y + normalizedDirection * 0.01f;

        return _ladder.IsHaveLadderOnHeight(predictedPos);
    }
}

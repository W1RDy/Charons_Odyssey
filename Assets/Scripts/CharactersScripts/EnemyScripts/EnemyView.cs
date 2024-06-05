using UnityEngine;

public class EnemyView : IPause
{
    private Animator _animator;
    private StunAnimation _stunAnimation;
    private TakeHitAnimation _takeHitAnimation;

    private EntityHPBar _hpBar;

    public EnemyView(SpriteRenderer spriteRenderer, Animator animator, StunAnimation stunAnimation, TakeHitAnimation takeHitAnimation, EntityHPBar hpBar)
    {
        _animator = animator;
        _stunAnimation = stunAnimation;
        _takeHitAnimation = takeHitAnimation;

        _hpBar = hpBar;

        _stunAnimation.SetParameters(spriteRenderer.transform, spriteRenderer);
        _takeHitAnimation.SetParameters(spriteRenderer);
    }

    public void SetIdleAnimation()
    {
        foreach (var parameter in _animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
                _animator.SetBool(parameter.name, parameter.defaultBool);
        }
        SetTakeHitAnimation(false);
        SetStunAnimation(false);
    }

    public void SetAnimation(string animationIndex, bool isActivate)
    {
        if (animationIndex == "Idle" && isActivate) SetIdleAnimation();
        else
        {
            if (animationIndex == "TakeHit") SetTakeHitAnimation(isActivate);
            else if (animationIndex == "Stun") SetStunAnimation(isActivate);
            else
            {
                try { _animator.SetBool(animationIndex, isActivate); }
                catch { if (isActivate) _animator.SetTrigger(animationIndex); }
            }
        }
    }

    public void ChangeHPView(float hp)
    {
        _hpBar.SetHP(hp);
    }

    public float GetAnimationDuration()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public string GetAnimationName()
    {
        return _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }

    private void SetTakeHitAnimation(bool isActivate)
    {
        if (isActivate)
        {
            _takeHitAnimation.Play();
            if (!GetAnimationName().EndsWith("Attack")) _animator.SetTrigger("TakeHit");
        }
        else _takeHitAnimation.Kill();
    }

    private void SetStunAnimation(bool isActivate)
    {
        if (isActivate) _stunAnimation.Play();
        else _stunAnimation.Kill();

        _animator.SetBool("Stun", isActivate);
    }

    public void Pause()
    {
        _animator.speed = 0;
    }

    public void Unpause()
    {
        _animator.speed = 1;
    }
}
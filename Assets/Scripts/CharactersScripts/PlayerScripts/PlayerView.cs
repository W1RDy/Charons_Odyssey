using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerView : MonoBehaviour, IPause
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Animator _animator;
    private float _animatorSpeed;

    [SerializeField] private StunAnimation _stunAnimation;
    [SerializeField] private TakeHitAnimation _takeHitAnimation;

    [SerializeField] private ParticleSystem _particleSystem;

    [Inject]
    private void Construct(IInstantiator instantiator)
    {
        var pauseHandler = instantiator.Instantiate<PauseHandler>();
        pauseHandler.SetCallbacks(Pause, Unpause);
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _stunAnimation.SetParameters(_spriteRenderer.transform, _spriteRenderer);
        _takeHitAnimation.SetParameters(_spriteRenderer);
    }

    private void SetIdleAnimation()
    {
        foreach (var parameter in _animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
                _animator.SetBool(parameter.name, parameter.defaultBool);
            else if (parameter.type == AnimatorControllerParameterType.Int)
                _animator.SetInteger(parameter.name, parameter.defaultInt);
        }

        SetStunAnimation(false);
        SetTakeHitAnimation(false);
    }

    public void SetAnimation(string animationIndex, bool isActivate)
    {
        if (animationIndex == "Idle" && isActivate) SetIdleAnimation();
        else
        {
            if (animationIndex == "TakeHit") SetTakeHitAnimation(isActivate);
            else if (animationIndex == "Stun") SetStunAnimation(isActivate);

            try { _animator.SetBool(animationIndex, isActivate); }
            catch { if (isActivate) _animator.SetTrigger(animationIndex); }
        }
    }

    public float GetAnimationDuration()
    {
        if (_animator == null) return 0f;
        return _animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public string GetAnimationName()
    {
        if (_animator == null) return null;
        return _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }

    public void SetAnimatorSpeed(float speed)
    {
        if (_animator) _animator.speed = speed;
    }

    private void SetStunAnimation(bool isActivate)
    {
        if (isActivate) _stunAnimation.Play();
        else _stunAnimation.Kill();
    }

    private void SetTakeHitAnimation(bool isActivate)
    {
        if (isActivate) _takeHitAnimation.Play();
        else _takeHitAnimation.Kill();
    }

    public void FlipParticleSystem(Vector2 direction)
    {
        if ((direction.x > 0 && _particleSystem.transform.localScale.x < 0) || (direction.x < 0 && _particleSystem.transform.localScale.x > 0))
            _particleSystem.transform.localScale = new Vector3(-1 * _particleSystem.transform.localScale.x, 1, 1);
    }

    public void Pause()
    {
        _animatorSpeed = _animator.speed;
        SetAnimatorSpeed(0);
    }

    public void Unpause()
    {
        SetAnimatorSpeed(_animatorSpeed);
    }
}

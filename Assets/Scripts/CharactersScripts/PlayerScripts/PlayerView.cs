using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerView : MonoBehaviour, IPause
{
    private Animator _animator;
    private PauseService _pauseService;
    private float _animatorSpeed;

    [Inject]
    private void Construct(PauseService pauseService)
    {
        _pauseService = pauseService;
        _pauseService.AddPauseObj(this);
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
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
    }

    public void SetAnimation(string animationIndex, bool isActivate)
    {
        if (animationIndex == "Idle" && isActivate) SetIdleAnimation();
        else
        {
            try { _animator.SetBool(animationIndex, isActivate); }
            catch { if (isActivate) _animator.SetTrigger(animationIndex); }
        }
    }

    public float GetAnimationDuration()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public string GetAnimationName()
    {
        return _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }

    public void SetAnimatorSpeed(float speed)
    {
        _animator.speed = speed;
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

    public void OnDestroy()
    {
        _pauseService.RemovePauseObj(this);
    }
}

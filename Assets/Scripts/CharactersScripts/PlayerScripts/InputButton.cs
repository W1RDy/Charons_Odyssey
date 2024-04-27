public class InputButton
{
    private InputButtonType _buttonType;
    private bool _isPushed;

    public InputButton(InputButtonType buttonType)
    {
        _buttonType = buttonType;
    }

    public InputButtonType GetInputButtonType()
    {
        return _buttonType;
    }

    public bool IsPushed() => _isPushed;

    public void ChangeState(bool isPushed)
    {
        _isPushed = isPushed;
    }
}

public enum InputButtonType
{
    Move,
    Climb,
    Attack,
    HeavyAttack,
    Shot,
    Interact,
    Pause,
    Heal,
    Shield,
    Parrying,
    Dodge
}

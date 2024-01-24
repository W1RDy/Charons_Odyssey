public interface IInputService
{
    public void InitializeService();
    public bool ButtonIsPushed(InputButtonType buttonType);
    public void UpdateInputs();
}
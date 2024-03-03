public interface IHasStamina
{
    public void UseStamina(float value);
    public void RefillStamina(float value);
    public void ChangeStaminaTo(float value);
    public float GetStamina();
}
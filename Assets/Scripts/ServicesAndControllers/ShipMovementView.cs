public class ShipMovementView
{
    private Wheel _wheel;
    private Background _background;

    public ShipMovementView(Wheel wheel, Background background)
    {
        _wheel = wheel;
        _background = background;
    }

    public void ActivateMovementView()
    {
        _wheel.ActivateRotating();
        _background.ActivateMovement();
    }

    public void DeactivateMovementView()
    {
        _wheel.DeactivateRotating();
        _background.DeactivateMovement();
    }
}
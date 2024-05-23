public class ShipMovementView
{
    private Wheel _wheel;
    private Background _background;

    private AudioMaster _audioMaster;

    public ShipMovementView(Wheel wheel, Background background, AudioMaster audioMaster)
    {
        _wheel = wheel;
        _background = background;

        _audioMaster = audioMaster;
    }

    public void ActivateMovementView()
    {
        _wheel.ActivateRotating();
        _background.ActivateMovement();

        _audioMaster.PlaySound("ShipMovement");
        _audioMaster.PlaySound("WheelMovement");
    }

    public void DeactivateMovementView()
    {
        _wheel.DeactivateRotating();
        _background.DeactivateMovement();

        _audioMaster.StopSound("ShipMovement");
        _audioMaster.StopSound("WheelMovement");
    }
}
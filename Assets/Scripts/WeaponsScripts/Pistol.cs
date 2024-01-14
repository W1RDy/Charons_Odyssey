using UnityEngine;

public class Pistol : Guns
{
    public PistolViewConfig View { get; private set; }

    public void Initialize(Transform weaponPoint, PistolViewConfig pistolView, BulletsCounterIndicator bulletsCounterIndicator)
    {
        base.Initialize(weaponPoint);
        View = pistolView;
        _bulletsCounterIndicator = bulletsCounterIndicator;
    }
}

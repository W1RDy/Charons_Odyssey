using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PatronsItem : SelfUsableItem
{
    [SerializeField] private int patronsCount;
    private Guns gun;
    private WeaponService _weaponService;

    [Inject]
    private void Construct(WeaponService weaponService)
    {
        _weaponService = weaponService;
    }

    private void Start()
    {      
        gun = _weaponService.GetWeapon(WeaponType.Pistol) as Guns;
        Debug.Log(gun);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        gun.AddPatrons(patronsCount);
    }
}

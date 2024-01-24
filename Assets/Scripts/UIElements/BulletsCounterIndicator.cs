using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BulletsCounterIndicator : CounterIndicator
{
    private BulletStick[] _bulletSticks;

    protected override void Awake()
    {
        base.Awake();
        _bulletSticks = GetComponentsInChildren<BulletStick>();
    }

    public override void SetCount(int count)
    {
        base.SetCount(count);
        for (int i = 0;  i < _bulletSticks.Length; i++)
        {
            if (i < count) _bulletSticks[i].ActivateBulletStick();
            else _bulletSticks[i].DeactivateBulletStick();
        }
    }
}

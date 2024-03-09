using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpIndicator : MonoBehaviour
{
    private Image _hpIndicator;

    private void Awake()
    {
        _hpIndicator = transform.GetChild(0).GetComponent<Image>();
    }

    public void SetHp(float hp)
    {
        _hpIndicator.fillAmount = hp / 100;
    }
}

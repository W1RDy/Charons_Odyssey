using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour, IHPBar
{
    private Image _hpFillImage;

    private void Awake()
    {
        _hpFillImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void SetHP(float hp)
    {
        _hpFillImage.fillAmount = hp / 100;
    }
}

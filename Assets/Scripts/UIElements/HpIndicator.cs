using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpIndicator : MonoBehaviour
{
    private Text _hpIndicator;

    private void Awake()
    {
        _hpIndicator = GetComponent<Text>();
    }

    public void SetHp(float hp)
    {
        _hpIndicator.text = "Çהמנמגüו: " + hp;
    }
}

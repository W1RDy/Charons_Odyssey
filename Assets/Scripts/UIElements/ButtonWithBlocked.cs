using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWithBlocked : Button
{
    private Text _buttonText;

    protected override void Awake()
    {
        base.Awake();
        _buttonText = GetComponentInChildren<Text>();
    }

    public void BlockButton()
    {
        _buttonText.color = Color.gray;
        interactable = false;
    }

    public void UnblockButton()
    {
        _buttonText.color = Color.white;
        interactable = true;
    }
}

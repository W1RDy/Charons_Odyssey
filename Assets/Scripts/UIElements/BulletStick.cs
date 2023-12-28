using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletStick : MonoBehaviour
{
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void ActivateBulletStick()
    {
        _image.enabled = true;
    }

    public void DeactivateBulletStick()
    {
        _image.enabled = false;
    }
}

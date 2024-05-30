using System.Collections.Generic;
using UnityEngine;

public class SpaceBordersChanger : MonoBehaviour
{
    [SerializeField] private List<Collider2D> _borders;
    [SerializeField] private bool _isActivated;

    private bool _isActivatedInDefault;

    private void Awake()
    {
        _isActivatedInDefault = _isActivated;
    }

    public void ActivateBorders()
    {
        ChangeBorders(!_isActivatedInDefault);
    }

    public void DeactivateBorders()
    {
        ChangeBorders(_isActivatedInDefault);
    }

    private void ChangeBorders(bool isActivate)
    {
        _isActivated = isActivate;
        foreach (var border in _borders)
        {
            border.enabled = isActivate;
        }
    }
}
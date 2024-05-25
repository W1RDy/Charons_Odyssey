using System.Collections.Generic;
using UnityEngine;

public class SpaceBordersChanger : MonoBehaviour
{
    [SerializeField] private List<Collider2D> _borders;

    public void ActivateBorders()
    {
        ChangeBorders(true);
    }

    public void DeactivateBorders()
    {
        ChangeBorders(false);
    }

    private void ChangeBorders(bool isActivate)
    {
        foreach (var border in _borders)
        {
            border.enabled = isActivate;
        }
    }
}
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LocationIndicator : MonoBehaviour
{
    [SerializeField] private Text _locationText;

    public void ActivateIndicator(string locationName)
    {
        SetLocationName(locationName);
        var sequence = DOTween.Sequence();

        sequence
            .Append(_locationText.DOFade(1, 0.5f))
            .AppendInterval(2f)
            .Append(_locationText.DOFade(0, 0.5f));
    }

    private void SetLocationName(string name)
    {
        _locationText.text = name;
    }
}
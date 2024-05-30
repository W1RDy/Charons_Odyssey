using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Tip : MonoBehaviour
{
    [SerializeField] private Text _tipText;

    [SerializeField] private FadeTextAnimation _appearAnimation;
    [SerializeField] private FadeTextAnimation _disappearAnimation;

    private void Awake()
    {
        _appearAnimation.SetParameters(_tipText);
        _disappearAnimation.SetParameters(_tipText);
    }

    public void ActivateTip(string tipText)
    {
        _tipText.text = tipText;
        if (_disappearAnimation.IsPlaying) _disappearAnimation.Kill();
        _appearAnimation.Play();
    }

    public void DeactivateTip()
    {
        _tipText.text = "";
        if (_appearAnimation.IsPlaying) _appearAnimation.Kill();
        _disappearAnimation.Play();
    }

    private void OnDestroy()
    {
        DeactivateTip();
        _disappearAnimation.Kill();
    }
}

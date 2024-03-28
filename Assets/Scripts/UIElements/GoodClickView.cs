using DG.Tweening;

public class GoodClickView : ClickView
{ 
    public override void ShowClick()
    {
        gameObject.SetActive(true);

        _scaleSequence = DOTween.Sequence();
        _colorSequence = DOTween.Sequence();

        _scaleSequence
            .Append(_view.DOScale(1, 1f))
            .AppendCallback(() =>
            {
                _colorSequence.Complete();
                gameObject.SetActive(false);
            });

        foreach (var spriteRenderer in _spriteRenderers)
        {
            _colorSequence
                .Join(spriteRenderer.DOFade(0, 1f));
        }

        _scaleSequence.Play();
        _colorSequence.Play();
    }
}

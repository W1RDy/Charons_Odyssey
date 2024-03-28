using DG.Tweening;

public class BadClickView : ClickView
{
    public override void ShowClick()
    {
        gameObject.SetActive(true);

        _scaleSequence = DOTween.Sequence();
        _colorSequence = DOTween.Sequence();

        for (int i = 0; i < 2; i++) 
        {
            _scaleSequence
                .Append(_view.DOScale(1, 0.3f))
                .Append(_view.DOScale(0.6f, 0.3f));
            if (i == 0)
            {
                _scaleSequence.AppendCallback(() => _colorSequence.Play());
            }
        }

        _scaleSequence
            .AppendCallback(() => {
                _colorSequence.Complete();
                gameObject.SetActive(false);
            });

        foreach (var spriteRenderer in _spriteRenderers)
        {
            _colorSequence
                .Join(spriteRenderer.DOFade(0, 0.6f));

            _colorSequence.Pause();
        }

        _scaleSequence.Play();
    }
}

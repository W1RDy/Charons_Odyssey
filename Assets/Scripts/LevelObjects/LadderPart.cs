using UnityEngine;

public class LadderPart : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private float _height;
    public float Height => _height;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _height = _spriteRenderer.sprite.bounds.size.y;
    }

    public void HideLadderPart()
    {
        _spriteRenderer.enabled = false;
    }

    public void ShowLadderPart()
    {
        _spriteRenderer.enabled = true;
    }
}

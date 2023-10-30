using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;

    void Start()
    {
        var camera = GetComponent<Camera>();

        // Определяем размер камеры в пикселях
        float camSize = camera.orthographicSize * 2 * sprite.sprite.pixelsPerUnit;
        Debug.Log(camSize);

        // Определяем размер спрайта в пикселях
        float spriteSize = sprite.sprite.bounds.size.x * sprite.sprite.pixelsPerUnit / camera.pixelWidth;
        Debug.Log(spriteSize);
    }
}

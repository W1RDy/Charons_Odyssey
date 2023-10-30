using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;

    void Start()
    {
        var camera = GetComponent<Camera>();

        // ���������� ������ ������ � ��������
        float camSize = camera.orthographicSize * 2 * sprite.sprite.pixelsPerUnit;
        Debug.Log(camSize);

        // ���������� ������ ������� � ��������
        float spriteSize = sprite.sprite.bounds.size.x * sprite.sprite.pixelsPerUnit / camera.pixelWidth;
        Debug.Log(spriteSize);
    }
}

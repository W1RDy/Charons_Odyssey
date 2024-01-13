using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

[Serializable]
public class BackgroundLayer : IMovable
{
    public string layerIndex;
    public BackgroundLayerPart[] layerParts;
    [Range(0, 10)] public float speed = 1;
    public float randomOffsetValue;
    private Transform _layer;
    private BackgroundLayerPart _leftPart;
    private float _cameraWidthInUints;

    public void InitLayer(Transform layer)
    {
        _layer = layer;
        layerParts = layerParts.OrderBy(transform => transform.partTransform.position.x).ToArray();
        for (int i = 0; i < layerParts.Length; i++)
        {
            var nextLayerIndex = i == 0 ? layerParts.Length - 1 : i - 1;
            var previousLayerIndex = i == layerParts.Length - 1 ? 0 : i + 1;
            layerParts[i].SetParts(layerParts[nextLayerIndex], layerParts[previousLayerIndex]);
            layerParts[i].partTransform.SetParent(_layer);

            var offset = GetRandomOffset();
            layerParts[i].partTransform.position = new Vector2(layerParts[i].partTransform.position.x + offset, layerParts[i].partTransform.position.y);
        }
        _leftPart = layerParts[0];
        _cameraWidthInUints = CustomCamera.Instance.GetCameraSizeInUints().x;
    }

    public void Move()
    {
        _layer.Translate(Vector2.left * speed * Time.deltaTime);
        UpdateLayerParts();
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    private void UpdateLayerParts()
    {
        if (_leftPart.partTransform.position.x < CustomCamera.Instance.GetCameraPos().x - _cameraWidthInUints * 2f)
        {
            var _offset = GetRandomOffset();
            _leftPart.partTransform.position = new Vector2(_leftPart.NextPart.partTransform.position.x + _leftPart.NextPart.LayerSize.x + _offset, _leftPart.partTransform.position.y);
            _leftPart = _leftPart.PreviousPart;
        }
    }

    private float GetRandomOffset()
    {
        if (randomOffsetValue != 0) return Random.Range(-randomOffsetValue, randomOffsetValue);
        return 0;
    }
}

[Serializable]
public class BackgroundLayerPart
{
    public Transform partTransform;
    public BackgroundLayerPart NextPart { get; private set; }
    public BackgroundLayerPart PreviousPart { get; private set; }
    public Vector2 LayerSize { get; private set; }

    public void SetParts(BackgroundLayerPart nextPart, BackgroundLayerPart previousPart)
    {
        foreach (var sprite in partTransform.GetComponentsInChildren<SpriteRenderer>())
        {
            if (LayerSize == null || sprite.sprite.bounds.size.x > LayerSize.x) 
                LayerSize = sprite.sprite.bounds.size;
        }
        NextPart = nextPart;
        PreviousPart = previousPart;
    }
}

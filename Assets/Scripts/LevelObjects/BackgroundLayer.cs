using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using Zenject;

[Serializable]
public class BackgroundLayer : IMovable
{
    public string layerIndex;
    [Range(0, 10)] public float speed = 1;
    public float[] randomOffsetRange;

    public BackgroundLayerPart[] layerParts;
    private BackgroundLayerPart _leftPart;
    private Transform _layer;

    private float _cameraWidthInUints;
    private CustomCamera _customCamera;

    public void InitLayer(Transform layer, CustomCamera customCamera)
    {
        _layer = layer;
        _customCamera = customCamera;
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
        _cameraWidthInUints = _customCamera.GetCameraSizeInUints().x;
    }

    public void Move()
    {
        _layer.Translate(Vector2.left * speed * Time.deltaTime);
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void UpdateLayerParts()
    {
        if (_leftPart.partTransform.position.x - _leftPart.LayerExtent.x > _customCamera.GetCameraPos().x - _cameraWidthInUints)
        {
            UpdateLeftLayer();
        }
        else if (_leftPart.NextPart.partTransform.position.x + _leftPart.NextPart.LayerExtent.x < _customCamera.GetCameraPos().x + _cameraWidthInUints)
        {
            UpdateRightLayer();
        }
    }

    private void UpdateRightLayer()
    {
        var offset = GetRandomOffset();
        var layerRightBorder = _leftPart.NextPart.partTransform.position.x + _leftPart.NextPart.LayerExtent.x;
        _leftPart.partTransform.position = new Vector2(layerRightBorder + _leftPart.LayerExtent.x + offset, _leftPart.partTransform.position.y);
        _leftPart = _leftPart.PreviousPart;
    }

    private void UpdateLeftLayer()
    {
        var offset = GetRandomOffset();
        var layerLeftBorder = _leftPart.partTransform.position.x - _leftPart.LayerExtent.x;
        _leftPart.NextPart.partTransform.position = new Vector2(layerLeftBorder - (_leftPart.NextPart.LayerExtent.x + offset), _leftPart.NextPart.partTransform.position.y);
        _leftPart = _leftPart.NextPart;
    }

    private float GetRandomOffset()
    {
        if (randomOffsetRange.Length == 2) return Random.Range(randomOffsetRange[0], randomOffsetRange[1]);
        else if (randomOffsetRange.Length > 0) throw new InvalidOperationException("RandomOffsetRange should have length 2 or 0!");
        return 0;
    }
}

[Serializable]
public class BackgroundLayerPart
{
    public Transform partTransform;
    public BackgroundLayerPart NextPart { get; private set; }
    public BackgroundLayerPart PreviousPart { get; private set; }
    public Vector2 LayerExtent { get; private set; }

    public void SetParts(BackgroundLayerPart nextPart, BackgroundLayerPart previousPart)
    {
        NextPart = nextPart;
        PreviousPart = previousPart;
        foreach (var sprite in partTransform.GetComponentsInChildren<SpriteRenderer>())
        {
            if (LayerExtent == null || sprite.sprite.bounds.extents.x > LayerExtent.x)
                LayerExtent = sprite.sprite.bounds.extents;
        }
    }
}

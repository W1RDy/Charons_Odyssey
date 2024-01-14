using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Background : MonoBehaviour
{
    [SerializeField] private BackgroundLayer[] _layers;
    [Range(0, 2), SerializeField] private int _backgroundSpeed = 1;
    private CustomCamera _customCamera;

    [Inject]
    private void Construct(CustomCamera customCamera)
    {
        _customCamera = customCamera;
    }

    private void Awake()
    {
        foreach (var layer in _layers)
        {
            var layerTransform = new GameObject(layer.layerIndex + "Layer").transform;
            layerTransform.SetParent(transform);
            layer.InitLayer(layerTransform, _customCamera);
            layer.speed *= _backgroundSpeed;
        }
    }

    private void Update()
    {
        foreach (var layer in _layers) layer.Move();
    }
}

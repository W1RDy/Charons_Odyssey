using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private BackgroundLayer[] _layers;
    [Range(0, 2), SerializeField] private int backgroundSpeed = 1;

    private void Awake()
    {
        foreach (var _layer in _layers)
        {
            var _layerTransform = new GameObject(_layer.layerIndex + "Layer").transform;
            _layerTransform.SetParent(transform);
            _layer.InitLayer(_layerTransform);
            _layer.speed *= backgroundSpeed;
        }
    }

    private void Update()
    {
        foreach (var _layer in _layers) _layer.Move();
    }
}

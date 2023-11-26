using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    public static CustomCamera Instance;
    public Camera MainCamera { get; private set; }

    private void Awake()
    {
        Instance = this;
        MainCamera = GetComponent<Camera>();
    }

    public Vector2 GetCameraSizeInUints()
    {
        var height = MainCamera.orthographicSize * 2;
        var width = height * Camera.main.aspect;
        return new Vector2(width, height);
    }

    public Vector2 GetCameraPos() => MainCamera.transform.position;
}

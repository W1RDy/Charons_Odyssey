using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseEventHandler
{
    public event Action<Vector2> Noise;

    public void MakeNoise(Vector2 noisePosition)
    {
        Noise?.Invoke(noisePosition);
    }
}

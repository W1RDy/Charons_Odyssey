using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Instantiator : ICustomInstantiator
{
    private IInstantiator _instantiator;

    public Instantiator(IInstantiator instantiator)
    {
        _instantiator = instantiator;
    }

    public T InstantiatePrefabForComponent<T>(T prefab, Vector2 spawnPosition) where T : MonoBehaviour
    {
        return InstantiatePrefabForComponent(prefab, spawnPosition, null);
    }

    public T InstantiatePrefabForComponent<T>(T prefab, Vector2 spawnPosition, Transform parent) where T : MonoBehaviour
    {
        return _instantiator.InstantiatePrefabForComponent<T>(prefab, spawnPosition, Quaternion.identity, parent);
    }
}

public class InstantiatorOnSurface : ICustomInstantiator
{
    private IInstantiator _instantiator;

    public InstantiatorOnSurface(IInstantiator instantiator)
    {
        _instantiator = instantiator;
    }

    public T InstantiatePrefabForComponent<T>(T prefab, Vector2 spawnPosition) where T : MonoBehaviour
    {
        return InstantiatePrefabForComponent(prefab, spawnPosition, null);
    }

    public T InstantiatePrefabForComponent<T>(T prefab, Vector2 spawnPosition, Transform parent) where T : MonoBehaviour
    {
        var surfacePos = FinderObjects.FindGroundSurfaceInDirection(spawnPosition, Vector2.down);
        return _instantiator.InstantiatePrefabForComponent<T>(prefab, surfacePos, Quaternion.identity, parent);
    }
}

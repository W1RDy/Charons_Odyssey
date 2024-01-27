using UnityEngine;

public interface ICustomInstantiator
{
    public T InstantiatePrefabForComponent<T>(T prefab, Vector2 spawnPosition) where T : MonoBehaviour;
    public T InstantiatePrefabForComponent<T>(T prefab, Vector2 spawnPosition, Transform parent) where T : MonoBehaviour;

}
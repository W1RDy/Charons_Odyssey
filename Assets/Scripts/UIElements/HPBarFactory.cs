using UnityEngine;
using Zenject;

public class HPBarFactory : ICustomFactory
{
    private const string PATH = "HPBar";

    private EntityHPBar _hpBarPrefab;
    private IInstantiator _instantiator;
    private Transform _container;

    public HPBarFactory(IInstantiator instantiator, Transform container)
    {
        _instantiator = instantiator;
        _container = container;
        LoadResources();
    }

    private void LoadResources()
    {
        _hpBarPrefab = Resources.Load<EntityHPBar>(PATH);
    }

    public MonoBehaviour Create(Vector2 position)
    {
        return Create(position, Quaternion.identity, null);
    }

    public MonoBehaviour Create(Vector2 position, Quaternion rotation, Transform parent)
    {
        var barObject = _instantiator.InstantiatePrefabForComponent<EntityHPBar>(_hpBarPrefab, position, rotation, parent);
        barObject.transform.SetParent(_container);

        return barObject;
    }
}
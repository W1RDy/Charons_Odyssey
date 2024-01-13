using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private LoadSceneManager _sceneManagerPrefab;
    [SerializeField] private Inventory _inventoryPrefab;

    public override void InstallBindings()
    {
        BindLoadSceneManager();
        BindInventory();
    }

    private void BindLoadSceneManager()
    {
        Container.Bind<LoadSceneManager>().FromComponentInNewPrefab(_sceneManagerPrefab).AsSingle();
    }

    private void BindInventory()
    {
        Container.Bind<Inventory>().FromComponentInNewPrefab(_inventoryPrefab).AsSingle();
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private LoadSceneManager _sceneManagerPrefab;
    [SerializeField] private Inventory _inventoryPrefab;
    [SerializeField] private DialogManager _dialogManager;
    [SerializeField] private WeaponService _weaponService;

    public override void InstallBindings()
    {
        BindInputService();
        BindLoadSceneManager();
        BindInventory();
        BindDialogManager();
        BindWeaponManager();
    }

    private void BindLoadSceneManager()
    {
        Container.Bind<LoadSceneManager>().FromComponentInNewPrefab(_sceneManagerPrefab).AsSingle();
    }

    private void BindInventory()
    {
        Container.Bind<Inventory>().FromComponentInNewPrefab(_inventoryPrefab).AsSingle();
    }

    private void BindDialogManager()
    {
        Container.Bind<DialogManager>().FromComponentInNewPrefab(_dialogManager).AsSingle();
    }

    private void BindWeaponManager()
    {
        Container.Bind<WeaponService>().FromComponentInNewPrefab(_weaponService).AsSingle();
    }

    private void BindInputService()
    {
        var inputService = new PCInputService();
        inputService.InitializeService();
        Container.Bind<IInputService>().To<PCInputService>().FromInstance(inputService).AsSingle();
    }
}

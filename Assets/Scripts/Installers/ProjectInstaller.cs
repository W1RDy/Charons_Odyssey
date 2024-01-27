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
        BindServices();
        BindManagers();
        BindInventory();
    }

    private void BindServices()
    {
        BindInputService();
        BindEnemyService();
        BindNPCService();
        BindItemService();
    }

    private void BindManagers()
    {
        BindLoadSceneManager();
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

    private void BindEnemyService()
    {
        var enemyService = new EnemyService();
        enemyService.InitializeService();
        Container.Bind<EnemyService>().FromInstance(enemyService).AsSingle();
    }

    private void BindNPCService()
    {
        var npcService = new NPCService();
        npcService.InitializeService();
        Container.Bind<NPCService>().FromInstance(npcService).AsSingle();
    }

    private void BindItemService()
    {
        var itemService = new ItemService();
        itemService.InitializeService();
        Container.Bind<ItemService>().FromInstance(itemService).AsSingle();
    }
}

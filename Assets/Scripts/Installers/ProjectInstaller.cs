using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private LoadSceneManager _sceneManagerPrefab;
    [SerializeField] private Inventory _inventoryPrefab;
    [SerializeField] private WeaponService _weaponService;
    [SerializeField] private ArmorItemsService _armorItemsService;
    [SerializeField] private AudioService _audioServicePrefab;

    public override void InstallBindings()
    {
        BindDataController();
        BindServices();
        BindLoadSceneManager();
        BindInventory();
    }

    private void BindServices()
    {
        BindPauseService();
        BindAudioService();

        BindInputService();
        BindDialogService();

        BindEnemyService();
        BindNPCService();

        BindItemService();
        BindWeaponService();
        BindArmorItemService();
    }

    private void BindArmorItemService()
    {
        Container.Bind<ArmorItemsService>().FromComponentInNewPrefab(_armorItemsService).AsSingle();
    }

    private void BindLoadSceneManager()
    {
        Container.Bind<LoadSceneManager>().FromComponentInNewPrefab(_sceneManagerPrefab).AsSingle();
    }

    private void BindInventory()
    {
        Container.Bind<Inventory>().FromComponentInNewPrefab(_inventoryPrefab).AsSingle();
    }

    private void BindWeaponService()
    {
        BindService(Container.InstantiatePrefabForComponent<WeaponService>(_weaponService));
    }

    private void BindInputService()
    {
        var inputService = new PCInputService();
        inputService.InitializeService();
        Container.Bind<IInputService>().To<PCInputService>().FromInstance(inputService).AsSingle();
    }

    private void BindEnemyService()
    {
        BindService(new EnemyService());
    }

    private void BindNPCService()
    {
        BindService(new NPCService());
    }

    private void BindItemService()
    {
        BindService(new ItemService());
    }

    private void BindDialogService()
    {
        BindService(new DialogService());
    }

    private void BindDataController()
    {
        var dataController = new DataController();
        dataController.LoadDatas();
        Container.Bind<DataController>().FromInstance(dataController).AsSingle();
    }

    private void BindPauseService()
    {
        var pauseService = new PauseService();
        BindService(pauseService);
    }

    private void BindAudioService()
    {
        var audioService = Instantiate(_audioServicePrefab);
        BindService(audioService);

        var audioMaster = new AudioMaster(audioService);
        BindService(audioMaster);
    }

    private void BindService<T>(T service) where T : IService
    {
        service.InitializeService();
        Container.Bind<T>().FromInstance(service).AsSingle();
    }
}

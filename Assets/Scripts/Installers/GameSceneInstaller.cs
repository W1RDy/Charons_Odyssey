using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    #region Player

    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Transform _spawnPosition;

    #endregion

    #region HUD

    [SerializeField] private HpIndicator _hpIndicator;
    [SerializeField] private BulletsCounterIndicator _bulletsCounterIndicator;
    [SerializeField] private CustomCamera _customCamera;
    [SerializeField] private StaminaIndicator _staminaIndicator;

    #endregion

    #region EnemiesAndNPCs

    [SerializeField] private NPCGroup _npcGroupPrefab;
    private NPCService _npcService;
    private NPCFactory _npcFactory;
    private EnemyService _enemyService;

    #endregion

    #region Dialog

    [SerializeField] private DialogCloudService _dialogCloudService;
    [SerializeField] private DialogUpdater _dialogUpdater;
    private TalkableFinderOnLevel _talkableFinder;

    #endregion

    #region Game

    [SerializeField] private GameLifeController _gameLifeController;
    [SerializeField] private LevelInitializer _levelInitializer;

    #endregion

    #region OtherServices

    [SerializeField] private ButtonService _buttonService;
    [SerializeField] private WindowActivator _windowActivator;
    [SerializeField] private Settings _settings;
    private AudioService _audioService;
    private ItemService _itemService;

    #endregion

    [Inject]
    private void Construct(EnemyService enemyService, NPCService npcService, ItemService itemService, AudioService audioService)
    {
        _enemyService = enemyService;
        _npcService = npcService;
        _itemService = itemService;
        _audioService = audioService;
    }

    public override void InstallBindings()
    {
        BindEssenceSpawner();

        BindWindowActivator();

        BindSettings();
        BindAudioPlayer();

        BindGameLifeController();

        BindServices();

        BindHUD();
        BindCustomCamera();

        BindDialogLifeController();
        BindDialogActivator();
        BindDialogUpdater();

        BindTalkableFinder();

        BindFactories();
        BindPlayer();
    }

    private void BindHUD()
    {
        BindHpIndicator();
        BindBulletsCounterIndicator();
        BindStaminaIndicator();
    }

    private void BindServices()
    { 
        BindDialogCloudService();
        BindButtonService();
    }

    private void BindFactories()
    {
        BindEnemyFactory();
        BindNPCFactory();
        BindNPCGroupFactory();
        BindItemFactory();
    }

    private void BindStaminaIndicator()
    {
        Container.Bind<StaminaIndicator>().FromInstance(_staminaIndicator).AsSingle();
    }

    private void BindPlayer()
    {
        Player player = new Instantiator(Container).InstantiatePrefabForComponent<Player>(_playerPrefab, _spawnPosition.position);
        Container.Bind<Player>().FromInstance(player).AsSingle();
        _talkableFinder.AddTalkable(player);
    }

    private void BindGameLifeController()
    {
        Container.Bind<GameLifeController>().FromInstance(_gameLifeController).AsSingle();
    }

    private void BindHpIndicator()
    {
        Container.Bind<HpIndicator>().FromInstance(_hpIndicator).AsSingle();
    }

    private void BindBulletsCounterIndicator()
    {
        Container.Bind<BulletsCounterIndicator>().FromInstance(_bulletsCounterIndicator).AsSingle().NonLazy();
    }

    private void BindDialogCloudService()
    {
        Container.Bind<DialogCloudService>().FromInstance(_dialogCloudService).AsSingle();
    }

    private void BindCustomCamera()
    {
        Container.Bind<CustomCamera>().FromInstance(_customCamera).AsSingle();
    }

    private void BindDialogActivator()
    {
        Container.BindInterfacesAndSelfTo<DialogActivator>().FromNew().AsSingle();
    }

    private void BindButtonService()
    {
        Container.Bind<ButtonService>().FromInstance(_buttonService).AsSingle();
    }

    private void BindDialogUpdater()
    {
        Container.Bind<DialogUpdater>().FromInstance(_dialogUpdater).AsSingle();
    }

    private void BindEssenceSpawner()
    {
        Container.Bind<LevelInitializer>().FromInstance(_levelInitializer).AsSingle();
    }

    private void BindEnemyFactory()
    {
        var enemyFactory = new EnemyFactory(_enemyService, new Instantiator(Container));
        Container.Bind<IEnemyFactory>().To<EnemyFactory>().FromInstance(enemyFactory).AsSingle();
    }

    private void BindNPCFactory()
    {
        _npcFactory = new NPCFactory(_npcService, new InstantiatorOnSurface(Container), _talkableFinder);
        Container.Bind<INPCFactory>().To<NPCFactory>().FromInstance(_npcFactory).AsSingle();
    }

    private void BindNPCGroupFactory()
    {
        var npcGroupFactory = new NPCGroupFactory(new Instantiator(Container), _npcFactory, _npcGroupPrefab, _talkableFinder);
        Container.Bind<INPCGroupFactory>().To<NPCGroupFactory>().FromInstance(npcGroupFactory).AsSingle();
    }

    private void BindItemFactory()
    {
        var itemFactory = new ItemFactory(new Instantiator(Container), _itemService);
        Container.Bind<IItemFactory>().To<ItemFactory>().FromInstance(itemFactory).AsSingle();
    }

    private void BindTalkableFinder()
    {
        _talkableFinder = new TalkableFinderOnLevel();
        Container.Bind<TalkableFinderOnLevel>().FromInstance(_talkableFinder).AsSingle();
    }

    private void BindDialogLifeController()
    {
        Container.Bind<DialogLifeController>().FromNew().AsSingle();
    }

    private void BindWindowActivator()
    {
        Container.Bind<WindowActivator>().FromInstance(_windowActivator).AsSingle();
    }

    private void BindAudioPlayer()
    {
        var audioPlayer = new AudioPlayer(_audioService, _settings);
        Container.Bind<AudioPlayer>().FromInstance(audioPlayer).AsSingle();
    }

    private void BindSettings()
    {
        Container.Bind<Settings>().FromInstance(_settings).AsSingle();
    }
}
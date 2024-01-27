using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private GameService _gameService;
    [SerializeField] private HpIndicator _hpIndicator;
    [SerializeField] private BulletsCounterIndicator _bulletsCounterIndicator;
    [SerializeField] private DialogCloudService _dialogCloudService;
    [SerializeField] private CustomCamera _customCamera;
    [SerializeField] private ButtonService _buttonService;
    [SerializeField] private DialogUpdater _dialogUpdater;
    [SerializeField] private LevelInitializer _levelInitializer;
    [SerializeField] private NPCGroup _npcGroupPrefab;
    private EnemyService _enemyService;
    private NPCService _npcService;
    private NPCFactory _npcFactory;
    private ItemService _itemService;
    private TalkableFinderOnLevel _talkableFinder;

    [Inject]
    private void Construct(EnemyService enemyService, NPCService npcService, ItemService itemService)
    {
        _enemyService = enemyService;
        _npcService = npcService;
        _itemService = itemService;
    }

    public override void InstallBindings()
    {
        BindEssenceSpawner();
        BindServices();
        BindHUD();
        BindCustomCamera();
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
    }

    private void BindServices()
    { 
        BindGameService();
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

    private void BindPlayer()
    {
        Player player = new Instantiator(Container).InstantiatePrefabForComponent<Player>(_playerPrefab, _spawnPosition.position);
        Container.Bind<Player>().FromInstance(player).AsSingle();
        _talkableFinder.AddTalkable(player);
    }

    private void BindGameService()
    {
        Container.Bind<GameService>().FromInstance(_gameService).AsSingle();
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
}
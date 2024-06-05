using System;
using UnityEngine;
using Zenject;

public class EntitiesInstaller : MonoInstaller
{
    #region Player

    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Transform _spawnPosition;

    #endregion

    #region EnemiesAndNPCs

    [SerializeField] private NPCGroup _npcGroupPrefab;
    private NPCService _npcService;
    private NPCFactory _npcFactory;
    private EnemyService _enemyService;

    #endregion

    private TalkableFinderOnLevel _talkableFinder;
    private ItemService _itemService;

    [SerializeField] private Canvas _entitiesUICanvas;

    [Inject]
    private void Construct(EnemyService enemyService, NPCService npcService, ItemService itemService, TalkableFinderOnLevel talkableFinder)
    {
        _enemyService = enemyService;
        _npcService = npcService;

        _itemService = itemService;
        _talkableFinder = talkableFinder;
    }

    public override void InstallBindings()
    {
        BindFactories();
        BindPlayer();
    }

    private void BindPlayer()
    {
        Player player = new Instantiator(Container).InstantiatePrefabForComponent<Player>(_playerPrefab, _spawnPosition.position);
        Container.Bind<Player>().FromInstance(player).AsSingle();
        _talkableFinder.AddTalkable(player);
    }

    private void BindFactories()
    {
        BindHPBarInitializer();
        BindEnemyFactory();
        BindNPCFactory();
        BindNPCGroupFactory();
        BindItemFactory();
    }

    private void BindHPBarInitializer()
    {
        var hpBarFactory = new HPBarFactory(Container, _entitiesUICanvas.transform);
        var hpBarInitializer = new HPBarInitializer(hpBarFactory);

        Container.Bind<HPBarInitializer>().FromInstance(hpBarInitializer).AsSingle();
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
}
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private GameService _gameService;
    [SerializeField] private HpIndicator _hpIndicator;
    [SerializeField] private BulletsCounterIndicator _bulletsCounterIndicator;
    [SerializeField] private DialogCloudService _dialogCloudService;

    public override void InstallBindings()
    {
        BindServices();
        BindHUD();
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
    }

    private void BindPlayer()
    {
        Player player = Container.InstantiatePrefabForComponent<Player>(_playerPrefab, _spawnPosition.position, Quaternion.identity, null);
        Container.Bind<Player>().FromInstance(player).AsSingle();
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
}
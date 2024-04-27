using Newtonsoft.Json.Bson;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    #region HUD

    [SerializeField] private HpIndicator _hpIndicator;
    [SerializeField] private BulletsCounterIndicator _bulletsCounterIndicator;
    [SerializeField] private CustomCamera _customCamera;
    [SerializeField] private StaminaIndicator _staminaIndicator;

    #endregion

    #region Dialog

    [SerializeField] private DialogCloudService _dialogCloudService;
    [SerializeField] private DialogUpdater _dialogUpdater;

    #endregion

    #region Game

    [SerializeField] private GameLifeController _gameLifeController;
    [SerializeField] private LevelInitializer _levelInitializer;

    #endregion

    #region OtherServices

    [SerializeField] private ButtonService _buttonService;
    [SerializeField] private WindowActivator _windowActivator;
    [SerializeField] private Settings _settings;
    [SerializeField] private SubscribeController _subscribeController;
    private AudioService _audioService;

    #endregion

    [Inject]
    private void Construct(AudioService audioService)
    {
        _audioService = audioService;
    }

    public override void InstallBindings()
    {
        BindSubscribeController();
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

        BindNoiseEventHandler();
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

    private void BindSubscribeController()
    {
        Container.Bind<SubscribeController>().FromInstance(_subscribeController).AsSingle();
    }

    private void BindNoiseEventHandler()
    {
        var noiseEventHandler = new NoiseEventHandler();
        Container.Bind<NoiseEventHandler>().FromInstance(noiseEventHandler).AsSingle();
    }

    private void BindStaminaIndicator()
    {
        Container.Bind<StaminaIndicator>().FromInstance(_staminaIndicator).AsSingle();
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

    private void BindTalkableFinder()
    {
        var talkableFinder = new TalkableFinderOnLevel();
        Container.Bind<TalkableFinderOnLevel>().FromInstance(talkableFinder).AsSingle();
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

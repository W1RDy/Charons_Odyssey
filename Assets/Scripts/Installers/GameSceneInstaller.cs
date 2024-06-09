using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    #region HUD

    [SerializeField] private PlayerHpBar _hpIndicator;
    [SerializeField] private BulletsCounterIndicator _bulletsCounterIndicator;
    [SerializeField] private CustomCamera _customCamera;
    [SerializeField] private StaminaIndicator _staminaIndicator;

    #endregion

    #region Dialog

    [SerializeField] private DialogCloudService _dialogCloudService;
    [SerializeField] private DialogUpdater _dialogUpdater;
    [SerializeField] private DialogClickHandler _dialogClickHandler;

    //private DialogLifeController _dialogLifeController;

    #endregion

    #region Game

    [SerializeField] private GameStateController _gameLifeController;
    [SerializeField] private LevelInitializer _levelInitializer;

    #endregion

    #region OtherServices

    [SerializeField] private ButtonService _buttonService;
    [SerializeField] private WindowActivator _windowActivator;

    [SerializeField] private Tip _tip;
    [SerializeField] private TipActivator _tipActivator;

    [SerializeField] private Settings _settings;
    [SerializeField] private SubscribeController _subscribeController;
    private AudioService _audioService;
    private AudioMaster _audioMaster;

    #endregion

    [Inject]
    private void Construct(AudioService audioService, AudioMaster audioMaster)
    {
        _audioService = audioService;
        _audioMaster = audioMaster;
    }

    public override void InstallBindings()
    {
        BindSubscribeController();
        BindEssenceSpawner();

        BindWindowActivator();
        BindTipActivator();

        BindSettings();
        BindAudioPlayer();

        BindGameStateController();
        BindBattleActivator();

        BindServices();

        BindHUD();
        BindCustomCamera();

        BindTalkableFinder();
 
        BindDialogLifeController();
        BindDialogActivator();
        BindDialogUpdater();

        BindNoiseEventHandler();
    }

    private void BindBattleActivator()
    {
        Container.Bind<BattleActivator>().FromNew().AsSingle();
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

    private void BindTipActivator()
    {
        var tipActivator = new TipActivator(_tip);
        Container.Bind<TipActivator>().FromInstance(tipActivator).AsSingle();
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

    private void BindGameStateController()
    {
        Container.Bind<GameStateController>().FromInstance(_gameLifeController).AsSingle();
    }

    private void BindHpIndicator()
    {
        Container.Bind<PlayerHpBar>().FromInstance(_hpIndicator).AsSingle();
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
        var _dialogLifeController = Container.Instantiate<DialogLifeController>(new object[] { _dialogClickHandler });
        Container.Bind<DialogLifeController>().FromInstance(_dialogLifeController).AsSingle();
    }

    private void BindWindowActivator()
    {
        Container.Bind<WindowActivator>().FromInstance(_windowActivator).AsSingle();
    }

    private void BindAudioPlayer()
    {
        _audioMaster.SetSettings(_settings);

        var mainAudioPlayer = _audioService.transform.GetChild(0).GetComponent<AudioPlayer>();
        Container.Inject(mainAudioPlayer);
    }

    private void BindSettings()
    {
        Container.Bind<Settings>().FromInstance(_settings).AsSingle();
        Container.Inject(_settings);
    }
}

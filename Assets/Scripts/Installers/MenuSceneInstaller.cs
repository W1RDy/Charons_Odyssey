using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class MenuSceneInstaller : MonoInstaller
{
    [SerializeField] ButtonService _buttonService;
    [SerializeField] WindowActivator _windowActivator;
    [SerializeField] private Settings _settings;
    [SerializeField] private SubscribeController _subscribeController;
    private AudioService _audioService;
    private AudioMaster _audioMaster;

    [Inject]
    private void Construct(AudioService audioService, AudioMaster audioMaster)
    {
        _audioService = audioService;
        _audioMaster = audioMaster;
    }

    public override void InstallBindings()
    {
        BindSubscribeController();
        BindSettings();
        BindAudioPlayer();
        BindWindowActivator();
        BindTalkableFinder();
        BindButtonService();
    }

    private void BindSubscribeController()
    {
        Container.Bind<SubscribeController>().FromInstance(_subscribeController).AsSingle();
    }

    private void BindWindowActivator()
    {
        Container.Bind<WindowActivator>().FromInstance(_windowActivator).AsSingle();
    }

    private void BindButtonService()
    {
        Container.Bind<ButtonService>().FromInstance(_buttonService).AsSingle();
    }

    private void BindTalkableFinder()
    {
        Container.Bind<TalkableFinderOnLevel>().FromNew().AsSingle();
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
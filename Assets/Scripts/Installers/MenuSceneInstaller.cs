using System.Collections;
using UnityEngine;
using Zenject;

public class MenuSceneInstaller : MonoInstaller
{
    [SerializeField] ButtonService _buttonService;
    [SerializeField] WindowActivator _windowActivator;
    [SerializeField] private Settings _settings;
    private AudioService _audioService;

    [Inject]
    private void Construct(AudioService audioService)
    {
        _audioService = audioService;
    }

    public override void InstallBindings()
    {
        BindSettings();
        BindAudioPlayer();
        BindWindowActivator();
        BindTalkableFinder();
        BindButtonService();
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
        var audioPlayer = new AudioPlayer(_audioService, _settings);
        Container.Bind<AudioPlayer>().FromInstance(audioPlayer).AsSingle();
    }

    private void BindSettings()
    {
        Container.Bind<Settings>().FromInstance(_settings).AsSingle();
    }
}
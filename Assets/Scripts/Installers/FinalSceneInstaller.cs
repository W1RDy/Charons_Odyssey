using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FinalSceneInstaller : MonoInstaller
{
    [SerializeField] ButtonService _buttonService;
    [SerializeField] WindowActivator _windowActivator;

    public override void InstallBindings()
    {
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
}

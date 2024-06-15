using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FinalSceneInstaller : MonoInstaller
{
    [SerializeField] ButtonService _buttonService;
    [SerializeField] WindowActivator _windowActivator;
    [SerializeField] private SubscribeController _subscribeController;

    public override void InstallBindings()
    {
        BindSubscribeController();
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
}

using System.Collections;
using UnityEngine;
using Zenject;

public class MenuSceneInstaller : MonoInstaller
{
    [SerializeField] ButtonService _buttonService;

    public override void InstallBindings()
    {
        BindButtonService();
    }

    private void BindButtonService()
    {
        Container.Bind<ButtonService>().FromInstance(_buttonService).AsSingle();
    }
}
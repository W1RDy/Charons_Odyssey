using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Zenject;

public class ButtonService : MonoBehaviour, IService, IPause
{
    private WindowActivator _windowActivator;
    private TalkableFinderOnLevel _talkableFinderOnLevel;
    private NPCTrader _trader;
    private LoadSceneManager _loadSceneManager;
    private DataController _dataController;
    private PauseService _pauseService;

    private AudioMaster _audioMaster;

    [Inject]
    private void Construct(LoadSceneManager loadSceneManager, WindowActivator windowActivator, TalkableFinderOnLevel talkableFinderOnLevel, DataController dataController,
        PauseService pauseService, AudioMaster audioMaster)
    {
        _loadSceneManager = loadSceneManager;
        _windowActivator = windowActivator;
        _talkableFinderOnLevel = talkableFinderOnLevel;
        _dataController = dataController;
        _pauseService = pauseService;

        _audioMaster = audioMaster;
    }

    public void InitializeService()
    {

    }

    public void CloseTrade()
    {
        if (_trader == null) _trader = _talkableFinderOnLevel.GetTalkable("trader") as NPCTrader;
        _trader.CloseTrade();
        PlayButtonSound();
    }

    public void StartGame()
    {
        _loadSceneManager.LoadScene(_dataController.DataContainer.lastLocationIndex);
        PlayButtonSound();
    }

    public void NewGame()
    {
        if (_dataController.DataContainer.isHaveSavings) _windowActivator.ActivateWindow(WindowType.ResetDataWindow);
        else ResetDatasAndStartGame();
        PlayButtonSound();
    }

    public void ResetDatasAndStartGame()
    {
        _dataController.ResetDatas();
        StartGame();
        PlayButtonSound();
    }

    public void Continue()
    {
        _pauseService.SetUnpause();
        DeactivateSettings();
        DeactivateControlling();
        DeactivatePauseWindow();
        PlayButtonSound();
    }

    public void DeactivatePauseWindow()
    {
        _windowActivator.DeactivateWindow(WindowType.PauseWindow);
        PlayPauseSound();
    }

    public void ActivatePauseWindow() // используется при отключении окна настроек в паузе
    {
        _windowActivator.ActivateWindow(WindowType.PauseWindow);
    }

    public void ActivateSettings()
    {
        _windowActivator.ActivateWindow(WindowType.SettingsWindow);
        PlayButtonSound();
    }

    public void DeactivateSettings()
    {
        _windowActivator.DeactivateWindow(WindowType.SettingsWindow);
        PlayButtonSound();
    }

    public void ActivateControlling()
    {
        _windowActivator.ActivateWindow(WindowType.ControllingWindow);
        PlayButtonSound();
    }

    public void DeactivateControlling()
    {
        _windowActivator.DeactivateWindow(WindowType.ControllingWindow);
        PlayButtonSound();
    }

    public void DeactivateResetDataWindow()
    {
        _windowActivator.DeactivateWindow(WindowType.ResetDataWindow);
        PlayButtonSound();
    }

    public void Menu()
    {
        _loadSceneManager.LoadScene(0);
        PlayButtonSound();
    }

    private void PlayButtonSound()
    {
        _audioMaster.PlaySound("Button");
    }

    private void PlayPauseSound()
    {
        _audioMaster.PlaySound("Pause");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        throw new System.NotImplementedException();
    }

    public void Unpause()
    {
        throw new System.NotImplementedException();
    }
}

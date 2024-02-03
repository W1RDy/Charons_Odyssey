using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Zenject;

public class ButtonService : MonoBehaviour, IService
{
    private WindowActivator _windowActivator;
    private TalkableFinderOnLevel _talkableFinderOnLevel;
    private NPCTrader _trader;
    private LoadSceneManager _loadSceneManager;
    private DataController _dataController;

    [Inject]
    private void Construct(LoadSceneManager loadSceneManager, WindowActivator windowActivator, TalkableFinderOnLevel talkableFinderOnLevel, DataController dataController)
    {
        _loadSceneManager = loadSceneManager;
        _windowActivator = windowActivator;
        _talkableFinderOnLevel = talkableFinderOnLevel;
        _dataController = dataController;
    }

    public void InitializeService()
    {

    }

    public void CloseTrade()
    {
        if (_trader == null) _trader = _talkableFinderOnLevel.GetTalkable("trader") as NPCTrader;
        _trader.CloseTrade();
    }

    public void StartGame()
    {
        _loadSceneManager.LoadScene(_dataController.DataContainer.lastLocationIndex);
    }

    public void NewGame()
    {
        if (_dataController.DataContainer.isHaveSavings) _windowActivator.ActivateWindow(WindowType.ResetDataWindow);
        else ResetDatasAndStartGame();
    }

    public void ResetDatasAndStartGame()
    {
        _dataController.ResetDatas();
        StartGame();
    }

    public void ActivatePause()
    {
        _windowActivator.ActivateWindow(WindowType.PauseWindow);
        Time.timeScale = 0;
    }

    public void DeactivatePause()
    {
        _windowActivator.DeactivateWindow(WindowType.PauseWindow);
        Time.timeScale = 1;
    }

    public void ActivateSettings()
    {
        _windowActivator.ActivateWindow(WindowType.SettingsWindow);
        Time.timeScale = 0;
    }

    public void DeactivateSettings()
    {
        _windowActivator.DeactivateWindow(WindowType.SettingsWindow);
        Time.timeScale = 1;
    }

    public void DeactivateResetDataWindow()
    {
        _windowActivator.DeactivateWindow(WindowType.ResetDataWindow);
        Time.timeScale = 1;
    }

    public void Menu()
    {
        _loadSceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ButtonService : MonoBehaviour
{
    [SerializeField] private WindowActivator _windowActivator;
    [SerializeField] private NPCTrader _trader;
    private LoadSceneManager _loadSceneManager;

    [Inject]
    private void Construct(LoadSceneManager loadSceneManager)
    {
        _loadSceneManager = loadSceneManager;
    }

    public void CloseTrade()
    {
        _trader.CloseTrade();
    }

    public void StartGame()
    {
        _loadSceneManager.LoadScene(1);
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

    public void Menu()
    {
        _loadSceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}

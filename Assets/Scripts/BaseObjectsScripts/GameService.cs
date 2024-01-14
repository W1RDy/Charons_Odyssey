using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameService : MonoBehaviour
{
    [SerializeField] private int _levelIndex;
    [SerializeField] private WindowActivator _windowActivator;
    private LoadSceneManager _loadSceneManager;
    private Inventory _inventory;
    public int LevelIndex { get => _levelIndex; }

    [Inject]
    private void Construct(LoadSceneManager loadSceneManager, Inventory inventory)
    {
        _loadSceneManager = loadSceneManager;
        _inventory = inventory;
    }

    private void Start()
    {
        _inventory.RemoveItem(ItemType.MissionItem);
    }

    public void LoseGame()
    {
        _windowActivator.ActivateWindow(WindowType.LoseWindow);
        _loadSceneManager.ReloadScene(2 * 1000);
    }

    public void WinGame()
    {
        _loadSceneManager.LoadNextLevel();
    }
}

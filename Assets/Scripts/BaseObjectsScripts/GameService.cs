using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameService : MonoBehaviour
{
    [SerializeField] private WindowActivator _windowActivator;

    private void Start()
    {
        Inventory.Instance.RemoveItem(ItemType.MissionItem);
    }

    public void LoseGame()
    {
        _windowActivator.ActivateWindow(WindowType.LoseWindow);
        LoadSceneManager.Instance.ReloadScene(2 * 1000);
    }

    public void WinGame()
    {
        LoadSceneManager.Instance.LoadNextLevel();
    }
}

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
        StartCoroutine(Delayer.DelayCoroutine(2f, () => LoadSceneManager.Instance.ReloadScene()));
    }

    public void WinGame()
    {
        LoadSceneManager.Instance.LoadNextLevel();
    }
}

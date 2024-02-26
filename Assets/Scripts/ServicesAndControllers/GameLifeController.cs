using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameLifeController : MonoBehaviour, IPause
{
    private WindowActivator _windowActivator;
    private LoadSceneManager _loadSceneManager;
    private Inventory _inventory;
    private IInputService _inputService;
    private ButtonService _buttonService;
    private LevelInitializer _levelInitializer;
    private PauseService _pauseService;
    private AudioPlayer _audioPlayer;
    private bool _isPaused;

    [Inject]
    private void Construct(LoadSceneManager loadSceneManager, Inventory inventory, IInputService inputService, ButtonService buttonService,
        LevelInitializer levelInitializer, WindowActivator windowActivator, PauseService pauseService, AudioPlayer audioPlayer)
    {
        _loadSceneManager = loadSceneManager;
        _inventory = inventory;
        _inputService = inputService;
        _buttonService = buttonService;
        _levelInitializer = levelInitializer;
        _windowActivator = windowActivator;
        _audioPlayer = audioPlayer;
        _pauseService = pauseService;
        _pauseService.AddPauseObj(this);
    }

    private void Start()
    {
        _inventory.LoadInventory();
        _inventory.RemoveItem(ItemType.MissionItem);
        _levelInitializer.SpawnAllLevelObjects();
    }

    private void Update() 
    {
        if (_inputService.ButtonIsPushed(InputButtonType.Pause))
        {
            if (!_isPaused)
            {
                _pauseService.SetPause();
            }
            else
            {
                _buttonService.Continue();
            }
        }
    }

    public void LoseGame()
    {
        _windowActivator.ActivateWindow(WindowType.LoseWindow);
        _loadSceneManager.ReloadScene(2 * 1000);
        _audioPlayer.Unsubscribe();
    }

    public void WinGame()
    {
        _inventory.SaveInventory();
        _loadSceneManager.LoadNextScene();
        _audioPlayer.Unsubscribe();
    }

    public void Pause()
    {
        if (!_isPaused)
        {
            _isPaused = true;
        }
    }

    public void Unpause()
    {
        if (_isPaused)
        {
            _isPaused = false;
        }
    }

    public void OnDestroy()
    {
        _pauseService.RemovePauseObj(this);
    }
}

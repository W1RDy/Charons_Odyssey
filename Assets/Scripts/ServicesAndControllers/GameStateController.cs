using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameStateController : MonoBehaviour, IPause
{
    private LoadSceneManager _loadSceneManager;
    private Inventory _inventory;
    private IInputService _inputService;

    private WindowActivator _windowActivator;
    private ButtonService _buttonService;
    private LevelInitializer _levelInitializer;

    private AudioMaster _audioPlayer;

    private bool _isPaused;
    private PauseService _pauseService;

    [Inject]
    private void Construct(LoadSceneManager loadSceneManager, Inventory inventory, IInputService inputService, ButtonService buttonService,
        LevelInitializer levelInitializer, WindowActivator windowActivator, IInstantiator instantiator, PauseService pauseService, AudioMaster audioPlayer)
    {
        _loadSceneManager = loadSceneManager;
        _inventory = inventory;
        _inputService = inputService;

        _levelInitializer = levelInitializer;
        _buttonService = buttonService;
        _windowActivator = windowActivator;

        _audioPlayer = audioPlayer;

        var pauseHandler = instantiator.Instantiate<PauseHandler>();
        pauseHandler.SetCallbacks(Pause, Unpause);

        _pauseService = pauseService;
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
                ActivatePauseState();
            }
            else
            {
                DeactivatePauseState();
            }
        }
    }

    public void ActivatePauseState()
    {
        _pauseService.SetPause();
        //_audioPlayer.StopMusic();
    }

    public void DeactivatePauseState()
    {
        _buttonService.Continue();
        //_audioPlayer.ContinueMusic();
    }

    public void ActivateLoseState()
    {
        _windowActivator.ActivateWindow(WindowType.LoseWindow);
        _loadSceneManager.ReloadScene(2 * 1000);
        _audioPlayer.Unsubscribe();
    }

    public void ActivateWinState()
    {
        _inventory.SaveInventory();
        _loadSceneManager.LoadNextScene();
        _audioPlayer.Unsubscribe();
    }

    public void ActivateBattleState()
    {
        _audioPlayer.PlayMusic("BattleMusic");
    }

    public void ActivateResearchState()
    {
        _audioPlayer.PlayMusic("ResearchMusic");
    }

    public void ActivateDialogState()
    {
        _audioPlayer.PlayMusic("DialogMusic");
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
}

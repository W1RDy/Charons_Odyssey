using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelInfo[] _levels;
    public static LevelManager Instance;
    private Dictionary<int, LevelInfo> _levelsDictionary;
    private GameService _gameService;

    [Inject]
    private void Construct(GameService gameService)
    {
        _gameService = gameService;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitDictionary();
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(Instance);
    }

    private void InitDictionary()
    {
        _levelsDictionary = new Dictionary<int, LevelInfo>();

        foreach (var level in _levels) _levelsDictionary[level.index] = level;
    }

    public LevelInfo GetLevel(int index) => _levelsDictionary[index];

    public LevelInfo GetNextLevel()
    {
        var index = _gameService.LevelIndex;
        return GetLevel(index + 1);
    }
}

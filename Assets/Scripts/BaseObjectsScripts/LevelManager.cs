using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelInfo[] _levels;
    public static LevelManager Instance;
    private Dictionary<int, LevelInfo> _levelsDictionary;

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
        var buildIndex = SceneManager.GetActiveScene().buildIndex;
        return GetLevel(buildIndex + 1);
    }
}

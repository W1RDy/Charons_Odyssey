using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Audio Data", menuName = "New Data/New Audio Data")]
public class AudioData : ScriptableObject
{
    [SerializeField] private List<AudioGroup> _audioGroups = new List<AudioGroup>();

    public List<AudioGroup> AudioGroups => _audioGroups;
}

[Serializable]
public class AudioConfig
{
    [SerializeField] private string _index;
    [SerializeField] private AudioPlayerType _audioPlayerType;

    [SerializeField] private float _volume;

    [SerializeField] private AudioClip _audioClip;

    [SerializeField] AudioConfig[] _additiveConfigs;

    public string Index => _index;
    public AudioPlayerType AudioPlayerType => _audioPlayerType;
    public string GroupIndex { get; private set; }

    public float Volume => _volume;

    public AudioClip AudioClip => _audioClip;
    public int ClipsCount => _additiveConfigs.Length + 1;

    public void Init(string groupIndex)
    {
        GroupIndex = groupIndex;

        foreach (var audioConfig in _additiveConfigs)
        {
            audioConfig.Init(groupIndex, _index, _audioPlayerType);
        }
    }

    public void Init(string groupIndex, string index, AudioPlayerType audioPlayerType)
    {
        _index = index;
        _audioPlayerType = audioPlayerType;
        GroupIndex = groupIndex;
    }

    public AudioConfig RandomizeConfig()
    {
        var randomIndex = Random.Range(0, _additiveConfigs.Length + 1);
        return randomIndex == _additiveConfigs.Length ? this : _additiveConfigs[randomIndex];
    }
}

[Serializable]
public class AudioGroup
{
    [SerializeField] private AudioConfig[] _audioConfigs;

    [SerializeField] private string _groupIndex;

    public AudioConfig[] AudioConfigs => _audioConfigs;
    public string GroupIndex => _groupIndex;
}

public enum AudioPlayerType
{
    Main,
    Wheel,
    Ship,
    Location,
    Player
}
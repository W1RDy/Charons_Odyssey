using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioService : MonoBehaviour, IService
{
    [SerializeField] private AudioGroup[] _audioGroups;
    private Dictionary<string, Dictionary<string, AudioConfig>> _audios = new Dictionary<string, Dictionary<string, AudioConfig>>();
    public AudioConfig CurrentAudioConfig { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void InitializeService()
    {
        foreach (var audioGroup in _audioGroups)
        {
            if (!_audios.ContainsKey(audioGroup.GroupIndex))
            {
                _audios.Add(audioGroup.GroupIndex, new Dictionary<string, AudioConfig>());
            }

            foreach (var audioConfig in audioGroup.AudioConfigs)
            {
                _audios[audioGroup.GroupIndex].Add(audioConfig.Index, audioConfig);
            }
        }
    }

    public AudioConfig GetMusic(string index)
    {
        return _audios["music"][index];
    }

    public AudioConfig GetSound(string index)
    {
        return _audios["sounds"][index];
    }
}

[Serializable]
public class AudioConfig
{
    [SerializeField] private string _index;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _volume;

    public string Index => _index;
    public float Volume => _volume;
    public AudioClip AudioClip => _audioClip;

}

[Serializable]
public class AudioGroup
{
    [SerializeField] private AudioConfig[] _audioConfigs;
    [SerializeField] private string _groupIndex;

    public AudioConfig[] AudioConfigs => _audioConfigs;
    public string GroupIndex => _groupIndex;
}

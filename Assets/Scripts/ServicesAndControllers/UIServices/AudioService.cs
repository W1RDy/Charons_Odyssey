using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioService : MonoBehaviour, IService
{
    [SerializeField] private AudioData _audioData;
    private Dictionary<string, Dictionary<string, AudioConfig>> _audios = new Dictionary<string, Dictionary<string, AudioConfig>>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void InitializeService()
    {
        foreach (var audioGroup in _audioData.AudioGroups)
        {
            if (!_audios.ContainsKey(audioGroup.GroupIndex))
            {
                _audios.Add(audioGroup.GroupIndex, new Dictionary<string, AudioConfig>());
            }

            foreach (var audioConfig in audioGroup.AudioConfigs)
            {
                audioConfig.Init(audioGroup.GroupIndex);
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
        if (_audios["one shot sound"].TryGetValue(index, out var sound)) return sound;
        return _audios["looping sound"][index];
    }
}
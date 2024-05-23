using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioMaster : IService
{
    private AudioService _audioService;

    private MainAudioPlayer _mainPlayer;
    private Dictionary<AudioPlayerType, LoopingSoundsPlayer> _loopingSoundPlayers = new Dictionary<AudioPlayerType, LoopingSoundsPlayer>(); 

    private Settings _settings;

    public AudioMaster(AudioService audioService)
    {
        _audioService = audioService;
    }

    public void InitializeService()
    {

    }

    public void SetSettings(Settings settings)
    {
        _settings = settings;
        _settings.SettingsChanged += SetSettings;
    }

    public void AddAudioPlayer(AudioPlayer audioPlayer)
    {
        if (audioPlayer.AudioPlayerType == AudioPlayerType.Main)
        {
            _mainPlayer = audioPlayer as MainAudioPlayer;
            _mainPlayer.SetVolume(_settings.MusicVolume);
        }
        else
        {
            _loopingSoundPlayers.Add(audioPlayer.AudioPlayerType, audioPlayer as LoopingSoundsPlayer);
            audioPlayer.SetVolume(_settings.SoundVolume);
        }
    }

    public void RemoveAudioPlayer(AudioPlayer audioPlayer)
    {
        if (audioPlayer.AudioPlayerType == AudioPlayerType.Main)
        {
            _mainPlayer = null;
        }
        else
        {
            _loopingSoundPlayers.Remove(audioPlayer.AudioPlayerType);
        }
    }

    public void SetSettings()
    {
        _mainPlayer.SetVolume(_settings.MusicVolume);
        foreach (var soundPlayer in _loopingSoundPlayers.Values)
        {
            soundPlayer.SetVolume(_settings.SoundVolume);
        }
    }

    public void PlayMusic(string index)
    {
        if (index == "") StopMusic();
        else
        {
            var audio = _audioService.GetMusic(index);
            _mainPlayer.PlayAudio(audio);
        }
    }

    public void StopMusic()
    {
        _mainPlayer.StopAudio();
    }

    public void PlaySound(string index)
    {
        var audio = _audioService.GetSound(index);
        if (audio.ClipsCount > 1) audio = audio.RandomizeConfig();

        if (audio.GroupIndex == "looping sound") PlayLoopingSound(audio);
        else if (audio.GroupIndex == "one shot sound") PlayOneShotSound(audio);
    }

    public void StopSound(string index)
    {
        var audio = _audioService.GetSound(index);
        if (audio.GroupIndex != "looping sound") return;

        var audioPlayer = _loopingSoundPlayers[audio.AudioPlayerType];
        audioPlayer.StopAudio();
    }

    private void PlayOneShotSound(AudioConfig audio)
    {
        _mainPlayer.PlaySound(audio.AudioClip, _settings.SoundVolume * audio.Volume);
    }

    private void PlayLoopingSound(AudioConfig audio)
    {
        var audioPlayer = _loopingSoundPlayers[audio.AudioPlayerType];
        audioPlayer.PlayAudio(audio);
    }

    public void Unsubscribe()
    {
        _settings.SettingsChanged -= SetSettings;
    }
}

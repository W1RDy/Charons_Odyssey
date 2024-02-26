using Unity.VisualScripting;
using UnityEngine;

public class AudioPlayer
{
    private AudioService _audioService;
    private AudioSource _audioSource;
    private Settings _settings;
    private string _rememberedIndex;

    public AudioPlayer(AudioService audioService, Settings settings)
    {
        _audioService = audioService;
        _audioSource = audioService.GetComponent<AudioSource>();
        _settings = settings;
        _settings.SettingsChanged += SetSettings;
    }

    public void SetSettings()
    {
        if (_audioService.CurrentAudioConfig != null)
        {
            _audioSource.volume = _settings.MusicVolume * _audioService.CurrentAudioConfig.Volume;
            if (_audioSource.volume == 0) StopMusic();
        }
        else if (_settings.MusicVolume > 0 && _rememberedIndex != "")
        {
            PlayMusic(_rememberedIndex);
        }
    }

    public void PlayMusic(string index)
    {
        _rememberedIndex = index;
        if ((_audioService.CurrentAudioConfig == null || _audioService.CurrentAudioConfig.Index != index) && _settings.MusicVolume > 0)
        {
            var audio = _audioService.GetMusic(index);
            StopMusic();
            PlayMusic(audio);
        }
    }

    private void StopMusic()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
            _audioService.CurrentAudioConfig = null;
        }
    }

    private void PlayMusic(AudioConfig audio)
    {
        _audioService.CurrentAudioConfig = audio;
        _audioSource.volume = _settings.MusicVolume * audio.Volume;
        _audioSource.clip = audio.AudioClip;
        _audioSource.Play();
    }

    public void PlaySound(string index)
    {
        if (_settings.SoundVolume > 0)
        {
            var audio = _audioService.GetSound(index);
            _audioSource.PlayOneShot(audio.AudioClip, _settings.SoundVolume * audio.Volume);
        }
    }

    public void Unsubscribe()
    {
        _settings.SettingsChanged -= SetSettings;
    }
}

using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public abstract class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioPlayerType _type;
    public AudioPlayerType AudioPlayerType => _type;

    protected AudioMaster _audioMaster;
    protected AudioSource _audioSource;
    protected AudioConfig _currentAudio;

    private float _volume;

    [Inject]
    private void Construct(AudioMaster audioMaster)
    {
        _audioSource = GetComponent<AudioSource>();

        _audioMaster = audioMaster;
        _audioMaster.AddAudioPlayer(this);
    }

    public virtual void PlayAudio(AudioConfig audio)
    {
        if (_audioSource.isPlaying && _audioSource.clip != audio.AudioClip)
        {
            StopAudio();
        }

        if (!_audioSource.isPlaying)
        {
            _audioSource.volume = _volume * audio.Volume;
            _audioSource.clip = audio.AudioClip;
            _currentAudio = audio;

            _audioSource.Play();
        }
    }

    public virtual void PlayAudio()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }

    public virtual void StopAudio()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }

    public void SetVolume(float volume)
    {
        if (_currentAudio != null) _audioSource.volume = volume * _currentAudio.Volume;
        _volume = volume;
    }

    public void OnDestroy()
    {
        _audioMaster.RemoveAudioPlayer(this);
    }
}

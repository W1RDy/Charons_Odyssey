using UnityEngine;

public class MainAudioPlayer : AudioPlayer
{
    public void PlaySound(AudioClip audioClip, float volume)
    {
        _audioSource.PlayOneShot(audioClip, volume);
    }

    public override void StopAudio()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Pause();
        }
    }
}
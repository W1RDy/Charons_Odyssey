using System.Collections.Generic;
using UnityEngine;

public class CompositeSoundsPlayer : LoopingSoundsPlayer
{
    private List<LoopingSoundsPlayer> _audioPlayers = new List<LoopingSoundsPlayer>();

    public void AddAudioPlayer(LoopingSoundsPlayer audioPlayer)
    {
        _audioPlayers.Add(audioPlayer);
    }

    public void RemoveAudioPlayer(LoopingSoundsPlayer audioPlayer)
    {
        _audioPlayers.Remove(audioPlayer);
    }

    public override void PlayAudio()
    {
        foreach (var audioPlayer in _audioPlayers)
        {
            audioPlayer.PlayAudio();
        }
    }

    public override void StopAudio()
    {
        foreach (var audioPlayer in _audioPlayers)
        {
            audioPlayer.StopAudio();
        }
    }

    public override void SetVolume(float volume)
    {
        foreach (var audioPlayer in _audioPlayers)
        {
            audioPlayer.SetVolume(volume);
        }
    }
}
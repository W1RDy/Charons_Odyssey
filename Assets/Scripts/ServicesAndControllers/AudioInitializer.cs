using UnityEngine;
using Zenject;

public class AudioInitializer : MonoBehaviour
{
    [SerializeField] private string _audioIndex;
    private AudioPlayer _audioPlayer;

    [Inject]
    private void Construct(AudioPlayer audioPlayer)
    {
        _audioPlayer = audioPlayer;
    }

    private void Start()
    {
        if (_audioIndex != "") _audioPlayer.PlayMusic(_audioIndex);
    }
}

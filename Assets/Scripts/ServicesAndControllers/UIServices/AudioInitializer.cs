using UnityEngine;
using Zenject;

public class AudioInitializer : MonoBehaviour
{
    [SerializeField] private string _musicIndex;
    [SerializeField] private string _soundIndex;

    private AudioMaster _audioMaster;

    [Inject]
    private void Construct(AudioMaster audioPlayer)
    {
        _audioMaster = audioPlayer;
    }

    private void Start()
    {
        if (_musicIndex == null || _musicIndex == "") _audioMaster.StopMusic();
        else _audioMaster.PlayMusic(_musicIndex);

        if (_soundIndex != null && _soundIndex != "") _audioMaster.PlaySound(_soundIndex);
    }
}

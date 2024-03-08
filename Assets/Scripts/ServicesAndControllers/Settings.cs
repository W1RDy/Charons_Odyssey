using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Settings : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;
    private DataController _dataController;
    public event Action SettingsChanged;

    public float MusicVolume { get; private set; }
    public float SoundVolume { get; private set; }

    [Inject]
    private void Construct(DataController dataController)
    {
        _dataController = dataController;
        MusicVolume = _dataController.DataContainer.musicVolume;
        SoundVolume = _dataController.DataContainer.soundsVolume;

        _musicSlider.value = MusicVolume;
        _soundSlider.value = SoundVolume;
    }

    public void ChangeMusicSetting()
    {
        if (_musicSlider.value != MusicVolume)
        {
            MusicVolume = _musicSlider.value;
            _dataController.DataContainer.musicVolume = MusicVolume;
            _dataController.SaveDatas();
            SettingsChanged?.Invoke();
        }
    }

    public void ChangeSoundSetting()
    {
        if (_soundSlider.value != SoundVolume)
        {
            SoundVolume = _soundSlider.value;
            _dataController.DataContainer.soundsVolume = SoundVolume;
            _dataController.SaveDatas();
            SettingsChanged?.Invoke();
        }
    }
}

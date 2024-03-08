using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseService : IService
{
    private List<IPause> _pauseObjs = new List<IPause>();
    private bool _isCanSetPause = true;

    public void InitializeService()
    {

    }

    public void SetPause()
    {
        if (_isCanSetPause)
        {
            foreach (IPause pauseObj in _pauseObjs)
            {
                pauseObj.Pause();
            }
        }
    }

    public void SetUnpause()
    {
        foreach (IPause pauseObj in _pauseObjs)
        {
            pauseObj.Unpause();
        }
    }

    public void AddPauseObj(IPause pauseObj)
    {
        _pauseObjs.Add(pauseObj);
    }

    public void RemovePauseObj(IPause pauseObj)
    {
        _pauseObjs.Remove(pauseObj);
    }

    public void ChangePauseAvailable(bool isCanSetPause)
    {
        _isCanSetPause = isCanSetPause;
    }
}

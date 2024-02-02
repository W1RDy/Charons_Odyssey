using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ButtonBlocker : MonoBehaviour
{
    [SerializeField] private ButtonWithBlocked _continueButton;
    private DataController _dataController;

    [Inject]
    private void Construct(DataController dataController)
    {
        _dataController = dataController;
    }

    private void Start()
    {
        if (!_dataController.DataContainer.isHaveSavings) _continueButton.BlockButton();
        else _continueButton.UnblockButton();
    }
}

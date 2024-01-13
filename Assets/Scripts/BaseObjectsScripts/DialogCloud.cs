using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogCloud : MonoBehaviour
{
    [SerializeField] private Text _message;

    public void SetMessage(string message)
    {
        _message.text = message;
    }
}

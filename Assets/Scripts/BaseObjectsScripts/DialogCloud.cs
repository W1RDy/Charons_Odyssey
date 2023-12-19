using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogCloud : MonoBehaviour
{
    [SerializeField] private Text _message;
    private int _symbolWidth = 10;
    private RectTransform _transform;

    private void Awake()
    {
        _transform = _message.GetComponent<RectTransform>();
    }

    public void SetMessage(string message)
    {
        _message.text = message;
        _transform.sizeDelta = new Vector2(_symbolWidth * message.Length, _transform.sizeDelta.y);
    }
}

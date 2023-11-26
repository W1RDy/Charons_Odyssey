using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goals : MonoBehaviour
{
    private Text _goals;

    private void Awake()
    {
        _goals = GetComponent<Text>();
    }

    public void SetGoals(string goals)
    {
        _goals.text = goals;
    }
}

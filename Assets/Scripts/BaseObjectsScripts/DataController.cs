using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DataController
{
    public DataContainer DataContainer { get; set; }

    public DataController()
    {
        DataContainer = new DataContainer();
    }

    public void SaveDatas()
    {
        if (!DataContainer.isHaveSavings) DataContainer.isHaveSavings = true;
        foreach (var field in typeof(DataContainer).GetFields())
        {
            if (field.FieldType == typeof(int)) PlayerPrefs.SetInt(field.Name, (int)field.GetValue(DataContainer));
            else if (field.FieldType == typeof(bool)) PlayerPrefs.SetString(field.Name, field.GetValue(DataContainer).ToString());
        }
    }

    public void LoadDatas()
    {
        if (!System.Convert.ToBoolean(PlayerPrefs.GetString("isHaveSavings", "false"))) return;
        foreach (var field in typeof(DataContainer).GetFields())
        {
            if (field.FieldType == typeof(int))
            {
                field.SetValue(DataContainer, PlayerPrefs.GetInt(field.Name));
            }
            else if (field.FieldType == typeof(bool))
            {
                var value = System.Convert.ToBoolean(PlayerPrefs.GetString(field.Name));
                field.SetValue(DataContainer, value);
            }
        }
    }

    public void ResetDatas()
    {
        if (DataContainer.isHaveSavings) DataContainer = new DataContainer();
        SaveDatas();
    }
}

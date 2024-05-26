using System.IO;
using UnityEditor;
using UnityEngine;

public class ConfigImportsMenu
{
    private const string CredentialsPath = "charons-odyssey-f6695769e714.json";
    private const string SpreadsheetId = "1YMhx3uEUB3Czt7ZFJdGi0Q33sO2JEo2rDnDwyptJcT8";
    private const string ItemsSheetsName = "Лист1";

    private const string DialogFilePath = "Configs/DialogFile.json";

    [MenuItem("Import/Imort Dialogs")]
    private static async void LoadDialogsFromGoogle()
    {
        var dialogConfigs = LoadDialogsFromJSON();
        var dialogParser = new DialogParser(dialogConfigs);

        var sheetsImporter = new GoogleSheetsImporter(CredentialsPath, SpreadsheetId);
        await sheetsImporter.DownloadAndParseSheet(ItemsSheetsName, dialogParser);

        SaveDialogsToJSON(dialogConfigs);
    }

    private static DialogConfigs LoadDialogsFromJSON()
    {
        var json = File.ReadAllText("Assets/Resources/" + DialogFilePath);
        var dialogConfigs = !string.IsNullOrEmpty(json) ? JsonUtility.FromJson<DialogConfigs>(json) : new DialogConfigs();
        return dialogConfigs;
    }

    private static void SaveDialogsToJSON(DialogConfigs dialogConfigs)
    {
        string resourcesPath = "Assets/Resources/" + DialogFilePath;
        var jsonString = JsonUtility.ToJson(dialogConfigs);
        File.WriteAllText(resourcesPath, jsonString);
    }
}

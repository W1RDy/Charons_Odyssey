using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using Cysharp.Threading.Tasks;

public static class DownloaderDataFromGoogleSheets
{
    public static async UniTask<Dictionary<string, DialogBranch>> DownloadDialogsData()
    {
        var rawJson = await GetValuesFromGoogleSheet("1YMhx3uEUB3Czt7ZFJdGi0Q33sO2JEo2rDnDwyptJcT8");
        var dataSet = new Dictionary<string, DialogBranch>();
        DialogBranch branch = null;
        MessageConfig lastMessageConfig = null;

        foreach (var itemRawJson in rawJson["values"])
        {
            var parseJson = JSON.Parse(itemRawJson.ToString());
            var selectRow = parseJson[0].AsStringList;
            if (selectRow[0].StartsWith("branch_"))
            {
                lastMessageConfig = null;
                branch = new DialogBranch(selectRow[0].Remove(0, 7), new List<MessageConfig>());
                dataSet.Add(branch.index, branch);
                continue;
            }
            else if (selectRow[0] == "key") continue;

            var isNeedChangeTalkable = lastMessageConfig == null || lastMessageConfig.talkableIndex != selectRow[0];
            lastMessageConfig = new MessageConfig(selectRow[1], isNeedChangeTalkable, selectRow[0]);
            branch.messageConfigs.Add(lastMessageConfig);
        }
        return dataSet;
    }

    private static async UniTask<JSONNode> GetValuesFromGoogleSheet(string sheetIndex)
    {
        UnityWebRequest currentResponse = UnityWebRequest.Get($"https://sheets.googleapis.com/v4/spreadsheets/{sheetIndex}/values/Лист1?key=AIzaSyBC456hXyi8dgKhvbs8kut5Y94nhhtA6hw");
        await currentResponse.SendWebRequest();
        return JSON.Parse(currentResponse.downloadHandler.text);
    }
}

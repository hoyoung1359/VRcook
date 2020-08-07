using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
public struct Column
{
    public string name;
    public string value;

    public Column(string name, string value)
    {
        this.name = name;
        this.value = value;
    }
}

public struct Row
{
    public Column[] columns;

    public Row(int columnCount)
    {
        columns = new Column[columnCount];
    }
}

/// <summary>
/// Performs database queries through intermediate webserver on Azure
/// Seperating database server connection allows removal
/// of any related package dependency from this unity project
/// </summary>
public class DatabaseRequest : MonoBehaviour
{

    public delegate void SelectCallback(Row[] result);

    private const string SERVER_PATH = "https://youcancook-server.azurewebsites.net";
    //private const string SERVER_PATH = "localhost:3000";

    // Send get request to webserver which will perform sql query on database server and return result
    public void Select(string tableName, SelectCallback callback)
    {
        StartCoroutine(GetRequest($"{SERVER_PATH}/{tableName}", callback));
    }

    public void SelectMenuList(string keyword, SelectCallback callback)
    {
        Debug.Log($"Searching for menu with keyword: {keyword}");
        StartCoroutine(GetRequest($"{SERVER_PATH}/foodName?keyword={keyword}", callback));
    }

    public void SelectCookingStep(int foodID, SelectCallback callback)
    {
        Debug.Log($"Selecting all cooking step for food with ID = {foodID}");
        StartCoroutine(GetRequest($"{SERVER_PATH}/cookingStep?foodID={foodID}", callback));
    }

    private IEnumerator GetRequest(string uri, SelectCallback callback)
    {
        Debug.Log($"sending request with uri: {uri}");
        using(var request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest(); // wait for result

            // parse response
            var responseBody = request.downloadHandler.text.Trim();
            Debug.Log($"Response body: {responseBody}");

            if(responseBody.Length > 0)
            {
                var rows = responseBody.Split('\n'); // rows are seperated by newline
                var result = new Row[rows.Length];
                for (var rowIndex = 0; rowIndex < rows.Length; rowIndex++)
                {
                    var columns = rows[rowIndex].Split('#'); // columns are seperated by sharp
                    result[rowIndex] = new Row(columns.Length);
                    for (var columnIndex = 0; columnIndex < columns.Length; columnIndex++)
                    {
                        var columnData = columns[columnIndex].Split(':'); // each column is stored in format "[column name]:[column value]"
                        result[rowIndex].columns[columnIndex] = new Column(columnData[0], columnData[1]);
                    }
                }

                callback(result);
            }
            else
            {
                callback(null);
            }
        }
    }
}

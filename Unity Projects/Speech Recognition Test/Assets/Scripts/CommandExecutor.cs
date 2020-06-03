using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Uses VoiceRecognizer to parse commands and execute them
/// </summary>
[RequireComponent(typeof(VoiceRecognizer))]
[RequireComponent(typeof(DatabaseRequest))]
public class CommandExecutor : MonoBehaviour
{
    public Text text;
    private DatabaseRequest databaseRequest;

    void Start()
    {
        var recognizer = GetComponent<VoiceRecognizer>();
        recognizer.recognitionResultHandler += RecognitionResultHandler;

        databaseRequest = GetComponent<DatabaseRequest>();
    }

    private void RecognitionResultHandler(object sender, string result)
    {
        Debug.Log($"Recognized result: {result}");
        text.text = result;

        if(result.StartsWith("요청"))
        {
            Debug.Log("Starting request");
            databaseRequest.Select("testTable", SelectCallback);
        }
    }

    private void SelectCallback(Row[] result)
    {
        Debug.Log("Select callback");
        for(var rowIndex = 0; rowIndex < result.Length; rowIndex++)
        {
            Debug.Log($"Row {rowIndex}");
            foreach(var column in result[rowIndex].columns)
            {
                Debug.Log($"{column.name}:{column.value}");
            }
        }
    }
}

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
        
        if (result.StartsWith("타이머"))
        {
            int time = 0;
            Debug.Log("Starting timer");
            string timeresult = result.Substring(0, result.Length-1);
            string[] timeList = timeresult.Split(' ');
            for(int i=0; i<timeList.Length ; i++)
            {
                if (timeList[i].EndsWith("시간"))
                {
                    try
                    {
                        int hour = Int32.Parse(timeList[i].Substring(0, timeList[i].Length - 2));
                        time += hour * 3600;
                    }
                    catch (Exception)
                    {
                        Debug.Log($"Unable to parse '{timeList[i]}'");
                    }
                }
                else if (timeList[i].EndsWith("분"))
                {
                    try
                    {
                        int minute = Int32.Parse(timeList[i].Substring(0, timeList[i].Length - 1));
                        time += minute * 60;
                    }
                    catch (Exception)
                    {
                        Debug.Log($"Unable to parse '{timeList[i]}'");
                    }
                }
                else if (timeList[i].EndsWith("초"))
                {
                    try
                    {
                        int second = Int32.Parse(timeList[i].Substring(0, timeList[i].Length - 1));
                        time += second;
                    }
                    catch (Exception)
                    {
                        Debug.Log($"Unable to parse '{timeList[i]}'");
                    }
                }
            }
            Debug.Log(time);
        }

        if (result.StartsWith("요청"))
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

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
[RequireComponent(typeof(TimerManager))]
[RequireComponent(typeof(RecognitionVisualizer))]
public class CommandExecutor : MonoBehaviour
{
    private DatabaseRequest databaseRequest;
    private TimerManager timerManager;

    void Start()
    {
        var recognizer = GetComponent<VoiceRecognizer>();
        recognizer.recognitionResultHandler += RecognitionResultHandler;

        databaseRequest = GetComponent<DatabaseRequest>();
        timerManager = GetComponent<TimerManager>();
    }

    private void RecognitionResultHandler(object sender, string result)
    {
        if (result.StartsWith("타이머"))
        {
            try
            {
                timerManager.StartTimer(ParseTime(result));
            }
            catch(Exception e)
            {
                Debug.Log($"Failed to parse timer command('{result}') with error message: '{e.Message}'");
            }
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

    // Calculates total duration of timer command in seconds
    // The format for the command is "타이머 [x시간] [x분] [x초]."
    // where brackets imply conditional arguments
    // Any punctuation mark at the end is ignored
    //
    // Ex) "타이머 일분 30초." => 90
    //     "타이머 1시간!" => 3600
    private int ParseTime(string timerCommand)
    {
        var timeResult = timerCommand.Substring(0, timerCommand.Length - 1); // Remove punctuation mark
        var timeList = timeResult.Split(' ');
        var time = 0;

        foreach (var token in timeList)
        {
            if (token.EndsWith("시간"))
            {
                var numericalPart = token.Substring(0, token.Length - 2);
                var parseSucceeded = int.TryParse(numericalPart, out int hour);

                if(!parseSucceeded)
                {
                    Debug.Log($"Unable to parse '{token}'");

                    throw new NotImplementedException("handle numbers in korean (e.g. 열시간)");
                }

                time += hour * 3600;
            }
            else if (token.EndsWith("분"))
            {
                var numericalPart = token.Substring(0, token.Length - 1);
                var parseSucceeded = int.TryParse(numericalPart, out int minute);

                if (!parseSucceeded)
                {
                    Debug.Log($"Unable to parse '{token}'");

                    throw new NotImplementedException("handle numbers in korean (e.g. 삼십분)");
                }

                time += minute * 60;
            }
            else if (token.EndsWith("초"))
            {
                var numericalPart = token.Substring(0, token.Length - 1);
                var parseSucceeded = int.TryParse(numericalPart, out int seconds);

                if (!parseSucceeded)
                {
                    Debug.Log($"Unable to parse '{token}'");

                    throw new NotImplementedException("handle numbers in korean (e.g. 십오초)");
                }

                time += seconds;
            }
            else if(!token.Equals("타이머"))
            {
                throw new Exception($"unexpected token in timer command: {token}");
            }
        }

        Debug.Log($"Parsed time in seconds: {time}");

        return time;
    }
}

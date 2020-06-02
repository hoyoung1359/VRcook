using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Uses VoiceRecognizer to parse commands and execute them
/// </summary>
[RequireComponent(typeof(VoiceRecognizer))]
public class CommandExecutor : MonoBehaviour
{
    public Text text;

    void Start()
    {
        var recognizer = GetComponent<VoiceRecognizer>();
        recognizer.recognitionResultHandler += RecognitionResultHandler;
    }

    private void RecognitionResultHandler(object sender, string result)
    {
        Debug.Log($"Recognized result: {result}");
        text.text = result;
    }
}

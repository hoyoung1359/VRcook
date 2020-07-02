using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VoiceRecognizer))]
public class RecognitionVisualizer : MonoBehaviour
{
    public Text recognitionResultText;
    public Text recognizingText;
    private VoiceRecognizer voiceRecognizer;

    // Start is called before the first frame update
    void Start()
    {
        var recognizer = GetComponent<VoiceRecognizer>();
        recognizer.recognitionResultHandler += RecognitionResultHandler;
        recognizer.recognitionStartHandler += RecognitionStartHandler;
        recognizer.recognizingHandler += RecognizingHandler;
    }

    private void RecognizingHandler(object sender, string result)
    {
        recognizingText.text = result;
    }

    private void RecognitionStartHandler(object sender, EventArgs e)
    {
        Debug.Log("Recognition start");
    }

    private void RecognitionResultHandler(object sender, string result)
    {
        Debug.Log($"Recogntion end: {result}");
        recognitionResultText.text = result;
    }
}

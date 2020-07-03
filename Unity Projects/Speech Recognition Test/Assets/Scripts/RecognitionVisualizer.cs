using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VoiceRecognizer))]
public class RecognitionVisualizer : MonoBehaviour
{
    public GameObject recognitionResult;
    public LeanTweenType easeType;
    private VoiceRecognizer voiceRecognizer;
    private Text recognitionResultText;

    // Start is called before the first frame update
    void Start()
    {
        var recognizer = GetComponent<VoiceRecognizer>();
        recognizer.recognitionResultHandler += RecognitionResultHandler;
        recognizer.recognitionStartHandler += RecognitionStartHandler;
        recognizer.recognizingHandler += RecognizingHandler;

        recognitionResultText = recognitionResult.GetComponentInChildren<Text>();
        recognitionResult.transform.localScale = Vector3.zero;
    }

    private void RecognizingHandler(object sender, string result)
    {
        recognitionResultText.text = result;
    }

    private void RecognitionStartHandler(object sender, EventArgs e)
    {
        Debug.Log("Recognition start");
        LeanTween.scale(recognitionResult, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
    }

    private void RecognitionResultHandler(object sender, string result)
    {
        Debug.Log($"Recogntion end: {result}");
        recognitionResultText.text = result;

        LeanTween.scale(recognitionResult, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInOutBack).setDelay(2.0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;

public class VoiceSynthesizer : MonoBehaviour
{
    private SpeechConfig speechConfig;
    private SpeechSynthesizer speechSynthesizer;

    void Start()
    {
        speechConfig = SpeechConfig.FromSubscription(
            AzureSubscription.SUBSCRIPTION_KEY,
            AzureSubscription.SUBSCRIPTION_REGION
        );
        speechConfig.SetProperty(
            PropertyId.SpeechServiceConnection_SynthLanguage,
            AzureSubscription.LANGUAGE_CODE
        );

        speechSynthesizer = new SpeechSynthesizer(speechConfig);

        /*
        speechSynthesizer.SynthesisCanceled += OnSynthesisCanceled;
        speechSynthesizer.SynthesisStarted += OnSynthesisStarted;
        speechSynthesizer.SynthesisCompleted += OnSynthesisCompleted;
        speechSynthesizer.Synthesizing += OnSynthesizing;
        //*/
    }

    /*
    private void OnSynthesizing(object sender, SpeechSynthesisEventArgs e)
    {
        Debug.Log("OnSynthesizing");
    }

    private void OnSynthesisCompleted(object sender, SpeechSynthesisEventArgs e)
    {
        Debug.Log("OnSynthesisCompleted");
    }

    private void OnSynthesisStarted(object sender, SpeechSynthesisEventArgs e)
    {
        Debug.Log("OnSynthesisStarted");
    }

    private void OnSynthesisCanceled(object sender, SpeechSynthesisEventArgs e)
    {
        Debug.Log($"OnSynthesisCanceled: {e.Result.Reason}");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            Speak("가나다라마바사");
    }
    //*/

    public async void Speak(string text)
    {
        Debug.Log($"Synthesizing voice from text: {text}");
        await speechSynthesizer.StartSpeakingTextAsync(text).ConfigureAwait(true);
    }
}

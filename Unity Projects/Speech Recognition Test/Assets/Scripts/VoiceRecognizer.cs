using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.IO;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

/// <summary>
/// Recognizes voice and convert to text through Azure Speech Recognition service.
/// Other components can access the recognition result by registering handlers
/// to recognitionResultHandler.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class VoiceRecognizer : MonoBehaviour
{
    // Azure subscription information constants
    private const string SUBSCRIPTION_KEY = "2a90829d0cdc4b9a96cb054796a0e6d2";
    private const string SUBSCRIPTION_REGION = "koreacentral";
    private const string LANGUAGE_CODE = "ko-KR";

    // Azure STT related variables
    private SpeechRecognizer recognizer;
    private PushAudioInputStream pushStream;
    private SpeechConfig speechConfig;
    private AudioConfig audioCongif;
    private AudioSource audioSource;                        // Recorded audio data is stored in its clip
    private string deviceName;                              // Name of recording device

    private int lastSamplePos = 0;                          // [lastSamplePos ~ currentSamplePos] is sent to Azure
    private bool isListening = false;                       // Set to true when SpeechRecognizer is running
    private object threadLocker = new object();             // Used for synchronization


    // Event handling
    public EventHandler recognitionStartHandler;            // An event that happens when recognition starts

    public EventHandler<string> recognizingHandler;         // An event that happens while recognizing a statement

    public EventHandler<string> recognitionResultHandler;   // An event that happens when a complete statement is recognized.
                                                            // The string parameter is the recognized result (including a punctuation mark)

    private string result;                                  // Lastest recognition result.

    private bool isRecognitionStarted = false;              // Recognition start event happens when this flag is set to true

    private bool isRecognizing = false;                     // Recognizing event happens when this flag is set to true.
                                                            // This flag is activated only while recognition is ongoing.

    private bool isResultReady = false;                     // Recognition result event handling happens when this flag is set to true.
                                                            // For some reason, using Unity Engine's features in SpeechRecognizer's
                                                            // callback functions doesn't seem to work.
                                                            // We instead handle it on regular Update() function by notifying
                                                            // that there exists a result that needs to be handled.

    private void Start()
    {
#if PLATFORM_ANDROID
        GetMicrophonePermission();
#endif

        deviceName = Microphone.devices[0];                 // Use the first device available
        audioSource = GetComponent<AudioSource>();

        InitializeRecognizer();
        StartRecognizing();
    }

    // Handle the actual event handlers.
    // We had to do this in indirect way because directly invoking
    // these handlers insidea VoiceRecognizer's callback didn't work.
    private void Update()
    {
        lock(threadLocker)
        {
            if(isRecognitionStarted)
            {
                isRecognitionStarted = false;
                recognitionStartHandler.Invoke(this, null);
            }

            if(isRecognizing)
            {
                recognizingHandler.Invoke(this, result);
            }

            if(isResultReady)
            {
                isResultReady = false;
                isRecognizing = false;
                recognitionResultHandler.Invoke(this, result);
            }
        }
    }

    private void FixedUpdate()
    {
        if (isListening)
        {
            var currentSamplePos = Microphone.GetPosition(deviceName);
            var sampleLength = currentSamplePos - lastSamplePos;

            if(sampleLength > 0)
            {
                // Get sample data
                var audioSample = new float[sampleLength * audioSource.clip.channels];
                audioSource.clip.GetData(audioSample, lastSamplePos);
                var byteData = ConvertAudioSampleTo16BitByte(audioSample);

                // If valid, send it to Azure for recognition
                if(byteData.Length > 0)
                {
                    pushStream.Write(byteData);

                    // Debug.Log($"Pushing audio sample with length: {byteData.Length} to Azure");
                }
            }

            lastSamplePos = currentSamplePos;
        }
    }

#if PLATFORM_ANDROID
    private void GetMicrophonePermission()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
    }
#endif

    private void InitializeRecognizer()
    {
        pushStream = AudioInputStream.CreatePushStream();
        audioCongif = AudioConfig.FromStreamInput(pushStream);

        speechConfig = SpeechConfig.FromSubscription(SUBSCRIPTION_KEY, SUBSCRIPTION_REGION);

        recognizer = new SpeechRecognizer(speechConfig, LANGUAGE_CODE, audioCongif);
        recognizer.Recognized += RecognizedHandler;
        recognizer.Recognizing += RecognizingHandler;
    }

    private void RecognizingHandler(object sender, SpeechRecognitionEventArgs e)
    {
        lock(threadLocker)
        {
            if(!isRecognizing)
            {
                isRecognitionStarted = true;
                isRecognizing = true;
            }
            result = e.Result.Text;
        }
    }

    private void RecognizedHandler(object sender, SpeechRecognitionEventArgs e)
    {
        lock(threadLocker)
        {
            result = e.Result.Text;
            isResultReady = true;
        }
    }

    private void StartRecording()
    {
        var length = 200;
        var frequency = 16000;
        audioSource.clip = Microphone.Start(deviceName, true, length, frequency);

        Debug.Log($"Microphone with device name: {deviceName} started recording with length: {length}, frequency: {frequency}");
    }

    private void StopRecording()
    {
        Microphone.End(deviceName);

        Debug.Log($"Microphone with device name [{deviceName}] stopped recording");
    }

    private async void StartRecognizing()
    {
        if (!Microphone.IsRecording(deviceName))
        {
            StartRecording();
        }

        await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(true);
        isListening = true;

        Debug.Log($"Recognizer started");
    }

    private async void StopRecognizing()
    {
        if(Microphone.IsRecording(deviceName))
        {
            StopRecording();
        }

        await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(true);
        isListening = false;

        Debug.Log($"Recognizer stopped");
    }

    private byte[] ConvertAudioSampleTo16BitByte(float[] audioSample)
    {
        MemoryStream dataStream = new MemoryStream();
        int x = sizeof(Int16);
        Int16 maxValue = Int16.MaxValue;
        int i = 0;
        while (i < audioSample.Length)
        {
            dataStream.Write(BitConverter.GetBytes(Convert.ToInt16(audioSample[i] * maxValue)), 0, x);
            ++i;
        }
        byte[] bytes = dataStream.ToArray();
        dataStream.Dispose();
        return bytes;
    }
}

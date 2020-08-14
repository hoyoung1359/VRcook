using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerDeleteButton : MonoBehaviour
{
    private GameObject notifier;
    private NotificationVisualizer notificationVisualizer;

    private GameObject voiceRecognizer;
    private TimerManager timerManager;
    private TimerDeleteListVisualizer timerDeleteListVisualizer;

    public Text text;

    public int timerIndex;

    public int TimerIndex
    {
        set
        {
            timerIndex = value;
            text.text = $"타이머 {timerIndex} 중지";
        }
    }

    private void Start()
    {
        notifier = GameObject.FindGameObjectWithTag("Notifier");
        notificationVisualizer = notifier.GetComponent<NotificationVisualizer>();

        voiceRecognizer = GameObject.FindGameObjectWithTag("VoiceRecognizer");
        timerManager = voiceRecognizer.GetComponent<TimerManager>();
        timerDeleteListVisualizer = voiceRecognizer.GetComponent<TimerDeleteListVisualizer>();
    }

    public void OnClick()
    {
        notificationVisualizer.Notify($"타이머 {timerIndex}을/를 중지했습니다");
        timerManager.DeleteTimer(timerIndex);
        timerDeleteListVisualizer.deactivate();
    }
}

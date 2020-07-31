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

    private Text text;

    public int timerIndex;

    private void Start()
    {
        notifier = GameObject.FindGameObjectWithTag("Notifier");
        notificationVisualizer = notifier.GetComponent<NotificationVisualizer>();

        voiceRecognizer = GameObject.FindGameObjectWithTag("VoiceRecognizer");
        timerManager = voiceRecognizer.GetComponent<TimerManager>();
        timerDeleteListVisualizer = voiceRecognizer.GetComponent<TimerDeleteListVisualizer>();
        text = GetComponentInChildren<Text>(true);
    }

    private void Update()
    {
        if(text == null)
        {
            text = GetComponentInChildren<Text>();
        }
        text.text = $"타이머 {timerIndex} 중지";
    }

    public void SetTimerIndex(int timerIndex)
    {
        Debug.Log($"Setting timer index of this button to {timerIndex}");
        this.timerIndex = timerIndex;

        //GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 40.0f) * timerIndex;
    }

    public void OnClick()
    {
        timerManager.DeleteTimer(timerIndex);
        timerDeleteListVisualizer.HideList(false);
        notificationVisualizer.Notify($"타이머 {timerIndex}을/를 중지했습니다");
    }
}

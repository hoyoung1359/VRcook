using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Timer))]
public class TimerInfoVisualizer : MonoBehaviour
{
    private const float UIWidth = 200.0f;
    private const float UIHeight = 40.0f;
    private const float transitionSpeed = 0.1f;

    private RectTransform rectTransform;
    private Text remainingTimeText;
    private Slider remainingTimeSlider;

    private Timer timer;

    private Vector3 offset;     // displacement from screen corner to the first timer UI's position
    private Vector3 padding;    // displacement between adjacent timer UIs (an upward vector)

    // This variable controls the horizontal position of the UI.
    // Timer 0 is the one at the top, followed by timer 1 right below and so on...
    // Timer manager will set this value on each update.
    public int timerIndex;

    void Start()
    {
        // Get UI components
        rectTransform = GetComponent<RectTransform>();
        remainingTimeText = GetComponentInChildren<Text>();
        remainingTimeSlider = GetComponentInChildren<Slider>();

        // Attach timer finish handler
        timer = GetComponent<Timer>();
        timer.timerFinishHandler += OnTimerFinish;

        // Calculate vectors for positioning
        offset = new Vector3(Screen.width - UIWidth / 2, Screen.height - UIHeight / 2);
        padding = new Vector3(0, UIHeight);

        // Initialize position so that the UI will come down from upper right border
        rectTransform.position = offset + padding;
    }

    void Update()
    {
        // Smoothly translate UI position to the target position
        var targetPos = offset - padding * timerIndex;
        var currentPos = rectTransform.position;
        rectTransform.position += (targetPos - currentPos) * transitionSpeed;

        // Show remaning time in text and progress bar
        remainingTimeText.text = $"({timerIndex}) {timer.RemainingTimeDescription()}";
        remainingTimeSlider.value = 1.0f - timer.RemainingTimeRatio();
    }

    private void OnTimerFinish(object sender, EventArgs e)
    {
        Debug.Log($"Timer {timerIndex} finished");
        // 상헌: 타이머 끝났다고 알림 소리 내기
        //       효과음은 아무거나 상관 없음 (나중에 바꾸면 됨)
    }
}

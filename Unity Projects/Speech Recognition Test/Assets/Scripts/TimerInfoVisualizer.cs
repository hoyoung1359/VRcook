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
    private const float transitionSpeed = 0.05f;

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
        // 타이머 끝나고 나서 아이콘을 바로 지워버리면 알아채기가 힘드니까 좀 놔뒀다가 사용자가 확인하면 삭제하는
        // 방식으로 해야 하는데 이것도 세부적으로 어떻게 처리할지 정해야 함
        //
        // Ex1) "타이머 1번 중지"같은 명령어로 알림 소리 끄고 아이콘 삭제
        // Ex2) 끝난 타이머 아이콘 3초 바라보면 삭제
        //
        // 일단 밑에 적어놨듯이 아이콘을 좀 눈에 띄게 바꾸고 알림 소리 내면 좋을듯
    }
}

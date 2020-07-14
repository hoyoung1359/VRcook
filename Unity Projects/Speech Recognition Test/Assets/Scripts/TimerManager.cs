using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public GameObject timerPrefab;
    public GameObject canvas;
    public GameObject createDeleteUI;
    private List<GameObject> timers;

    void Start()
    {
        timers = new List<GameObject>();
    }

    void Update()
    {
        for(int i=0;i<timers.Count;i++)
        {
            timers[i].GetComponent<TimerInfoVisualizer>().timerIndex = i;
        }
    }

    public void StartTimer(int seconds)
    {
        Debug.Log("Starting timer");

        var timer = Instantiate(timerPrefab, canvas.transform);
        timer.GetComponent<Timer>().Initialize(seconds);
        timers.Add(timer);
    }

    public void DeleteTimer(int timerIndex)
    {
        Debug.Log($"Deleting timer with index: {timerIndex}");
        // 상헌: 타이머를 리스트에서 삭제한다
    }

    public void ShowCreateDeleteUI()
    {
        Debug.Log("Showing timer create/delete UI");
        LeanTween.scale(createDeleteUI, Vector3.one, 0.5f).setEaseInOutExpo();
        // 타이머 시작/중지 버튼 띄우기
        // 시작 버튼 바라보면 isWaitingTimerCommand = true
        // 중지 버튼 바라보면 지금 돌아가는 타이머 목록을 버튼으로 보여주고
        // 그 중에서 바라본 타이머 중지
    }
}

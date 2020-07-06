using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public GameObject timerPrefab;
    public GameObject canvas;
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
        // 타이머를 리스트에서 삭제한다
        // 한 프레임 안에 타이머를 여럿 삭제하면 인덱스가 불일치하는 문제가 생긴다
        // 이런 경우가 발생하지 않는지 확인하자
    }
}

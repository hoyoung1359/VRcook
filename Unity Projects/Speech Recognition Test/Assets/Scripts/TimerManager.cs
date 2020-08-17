using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimerDeleteListVisualizer))]
public class TimerManager : MonoBehaviour
{
    public GameObject timerPrefab;
    public GameObject canvas;
    private List<GameObject> timers;
    private TimerDeleteListVisualizer timerDeleteListVisualizer;

    void Awake()
    {
        timers = new List<GameObject>();
        timerDeleteListVisualizer = GetComponent<TimerDeleteListVisualizer>();
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
        Destroy(timers[timerIndex]);
        timers.RemoveAt(timerIndex);
    }

    public int TimerListSize()
    {
        return timers.Count;
    }
}

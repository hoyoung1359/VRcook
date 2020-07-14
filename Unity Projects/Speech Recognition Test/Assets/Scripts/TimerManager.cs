using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimerDeleteListVisualizer))]
public class TimerManager : MonoBehaviour
{
    public GameObject timerPrefab;
    public GameObject canvas;
    public GameObject createDeleteUI;
    private List<GameObject> timers;
    private TimerDeleteListVisualizer timerDeleteListVisualizer;

    void Start()
    {
        timers = new List<GameObject>();
        timerDeleteListVisualizer = GetComponent<TimerDeleteListVisualizer>();

        StartTimer(30);
        StartTimer(20);
        StartTimer(10);
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

    public void ShowCreateDeleteUI()
    {
        Debug.Log("Showing timer create/delete UI");
        LeanTween.scale(createDeleteUI, Vector3.one, 0.5f).setEaseInOutExpo();
    }

    public void ShowTimerDeleteListUI()
    {
        timerDeleteListVisualizer.ShowList(timers.Count);
    }
}

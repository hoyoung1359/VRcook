using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float remainingTime;
    private int totalTime;

    public EventHandler timerFinishHandler;

    void Update()
    {
        if(remainingTime > 0.0f)
        {
            remainingTime -= Time.deltaTime;
            if(remainingTime <= 0.0f)
            {
                remainingTime = 0.0f;
                timerFinishHandler.Invoke(this, null);
            }
        }
    }

    public void Initialize(int seconds)
    {
        totalTime = seconds;
        remainingTime = seconds;
    }

    public float RemainingTimeRatio()
    {
        return remainingTime / totalTime;
    }

    public string RemainingTimeDescription()
    {
        var time = (int)Math.Round(remainingTime);
        var minutes = time / 60;
        var seconds = time % 60;

        return $"{minutes}:{seconds}";
    }
}

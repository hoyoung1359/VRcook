using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float remainingTime;
    private int totalTime;

    public Timer(int seconds)
    {
        totalTime = seconds;
        remainingTime = seconds;
    }

    // Decrease remaining time by specified deltaTime
    // Returns true if timer has expired
    public bool Progress(float deltaTime)
    {
        remainingTime -= deltaTime;

        return (remainingTime <= 0.0f);
    }

    public float RemainingTimeRatio()
    {
        return remainingTime / totalTime;
    }

    public int RemainingTime()
    {
        return (int)Math.Round(remainingTime);
    }

    public int TotalTime()
    {
        return totalTime;
    }
}

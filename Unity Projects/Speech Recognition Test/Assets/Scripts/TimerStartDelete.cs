using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerStartDelete : MonoBehaviour
{
    public UnityEvent timerStartPressedEvent;

    public void TimerStartButtonPressed()
    {
        Debug.Log("Timer start pressed");
        LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEaseInOutBack();
        timerStartPressedEvent.Invoke();
    }

    
    public void TimerDeleteButtonPressed()
    {
        Debug.Log("Timer delete pressed");
        LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEaseInOutBack();
    }
}

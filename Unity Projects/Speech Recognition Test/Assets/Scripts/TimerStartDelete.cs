using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerStartDelete : ScreenSpaceInteraction
{
    public TimerManager timerManager;
    public TimerDeleteListVisualizer timerDeleteListVisualizer;
    public NotificationVisualizer notificationVisualizer;
    public TimerDurationListener timerDurationListener;

    public override void onActivate()
    {
        Debug.Log("Showing timer create/delete UI");
        LeanTween.scale(gameObject, Vector3.one, 0.5f).setEaseInOutExpo();
    }

    public override void onDeactivate()
    {
        Debug.Log("Hiding timer create/delete UI");
        LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEaseInOutBack();
    }

    public void TimerStartButtonPressed()
    {
        notificationVisualizer.Notify("시간을 말해주세요(e.g., \"1분 30초\"");
        timerDurationListener.activate();
    }

    
    public void TimerDeleteButtonPressed()
    {
        notificationVisualizer.Notify("중지할 타이머를 선택해주세요");
        timerDeleteListVisualizer.ListSize = timerManager.TimerListSize();
        timerDeleteListVisualizer.activate();
    }
}

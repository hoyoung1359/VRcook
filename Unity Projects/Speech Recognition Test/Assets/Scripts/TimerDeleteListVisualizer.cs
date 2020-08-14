using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDeleteListVisualizer : ScreenSpaceInteraction
{
    public GameObject timerDeleteUIPrefab;
    public GameObject timerDeleteListParent;

    private int listSize;
    private bool instantly;
    private NotificationVisualizer notificationVisualizer;

    void Start()
    {
        notificationVisualizer = GameObject.FindGameObjectWithTag("Notifier").GetComponent<NotificationVisualizer>();
    }

    public int ListSize
    {
        set
        {
            listSize = value;
        }
    }

    public bool Instantly
    {
        set
        {
            instantly = value;
        }
    }


    public override void onActivate()
    {
        if (listSize == 0)
        {
            Debug.Log("Timer list is empty");
            notificationVisualizer.Notify("활성화된 타이머가 없습니다.");

            return;
        }
        Debug.Log("Showing list of deletable timers");
        DeleteChildButtons(); // Delete existing buttons, so that duplicate command will not cause duplicate buttons
        for (int i = 0; i < listSize; i++)
        {
            var timerDeleteUI = Instantiate(timerDeleteUIPrefab, timerDeleteListParent.transform);
            timerDeleteUI.GetComponent<TimerDeleteButton>().SetTimerIndex(i);
        }
        LeanTween.scale(timerDeleteListParent, Vector3.one, 0.5f).setEaseInExpo();
    }

    public override void onDeactivate()
    {
        Debug.Log("Hiding list of deletable timers");
        LeanTween.scale(timerDeleteListParent, Vector3.zero, instantly ? 0.0f : 0.5f).setEaseInOutBack().setOnComplete(DeleteChildButtons);
    }

    public void ShowList(int listSize)
    {
        this.activate();
    }

    public void HideList(bool instantly)
    {
        this.deactivate();
    }

    private void DeleteChildButtons()
    {
        foreach (Transform child in timerDeleteListParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}

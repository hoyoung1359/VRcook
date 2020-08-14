using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDeleteListVisualizer : ScreenSpaceInteraction
{
    public GameObject timerDeleteUIPrefab;
    public GameObject timerDeleteListParent;

    private int listSize;
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


    public override void onActivate()
    {
        if (listSize == 0)
        {
            notificationVisualizer.Notify("활성화된 타이머가 없습니다.");

            return;
        }

        // Create button instances
        for (int i = 0; i < listSize; i++)
        {
            var timerDeleteUI = Instantiate(timerDeleteUIPrefab, timerDeleteListParent.transform);
            timerDeleteUI.GetComponent<TimerDeleteButton>().TimerIndex = i;
        }

        LeanTween.scale(timerDeleteListParent, Vector3.one, 0.5f).setEaseInExpo();
    }

    public override void onDeactivate()
    {
        LeanTween.scale(timerDeleteListParent, Vector3.zero, 0.5f).setEaseInOutBack().setOnComplete(DeleteChildButtons);
    }

    private void DeleteChildButtons()
    {
        foreach (Transform child in timerDeleteListParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}

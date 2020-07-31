using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDeleteListVisualizer : MonoBehaviour
{
    public GameObject timerDeleteUIPrefab;
    public GameObject timerDeleteListParent;

    private NotificationVisualizer notificationVisualizer;

    void Start()
    {
        notificationVisualizer = GameObject.FindGameObjectWithTag("Notifier").GetComponent<NotificationVisualizer>();
    }

    public void ShowList(int listSize)
    {
        if(listSize == 0)
        {
            Debug.Log("Timer list is empty");
            notificationVisualizer.Notify("활성화된 타이머가 없습니다.");

            return;
        }
        Debug.Log("Showing list of deletable timers");
        DeleteChildButtons(); // Delete existing buttons, so that duplicate command will not cause duplicate buttons
        for(int i=0; i < listSize; i++)
        {
            var timerDeleteUI = Instantiate(timerDeleteUIPrefab, timerDeleteListParent.transform);
            timerDeleteUI.GetComponent<TimerDeleteButton>().SetTimerIndex(i);
        }
        LeanTween.scale(timerDeleteListParent, Vector3.one, 0.5f).setEaseInExpo();
    }

    public void HideList(bool instantly)
    {
        Debug.Log("Hiding list of deletable timers");
        LeanTween.scale(timerDeleteListParent, Vector3.zero, instantly ? 0.0f : 0.5f).setEaseInOutBack().setOnComplete(DeleteChildButtons);
    }

    private void DeleteChildButtons()
    {
        foreach (Transform child in timerDeleteListParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}

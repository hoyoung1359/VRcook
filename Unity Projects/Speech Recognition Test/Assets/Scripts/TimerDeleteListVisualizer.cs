using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDeleteListVisualizer : MonoBehaviour
{
    public GameObject timerDeleteUIPrefab;
    public GameObject timerDeleteListParent;

    public void ShowList(int listSize)
    {
        Debug.Log("Showing list of deletable timers");
        for(int i=0; i < listSize; i++)
        {
            var timerDeleteUI = Instantiate(timerDeleteUIPrefab, timerDeleteListParent.transform);
            timerDeleteUI.GetComponent<TimerDeleteButton>().SetTimerIndex(i);
        }
        LeanTween.scale(timerDeleteListParent, Vector3.one, 0.5f).setEaseInExpo();
    }

    public void HideList()
    {
        Debug.Log("Hiding list of deletable timers");
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

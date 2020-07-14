using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationVisualizer : MonoBehaviour
{
    public GameObject notification;
    private Text notificationText;
    private const float displayDuration = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        notificationText = notification.GetComponentInChildren<Text>();
        notification.transform.localScale = Vector3.zero;
    }

    public void Notify(string message)
    {
        notificationText.text = message;
        LeanTween.scale(notification, Vector3.one, 0.5f).setEaseInExpo().setOnComplete(HideNotification);
    }

    private void HideNotification()
    {
        LeanTween.scale(notification, Vector3.zero, 0.5f).setEaseInOutBack().setDelay(displayDuration);
    }
}

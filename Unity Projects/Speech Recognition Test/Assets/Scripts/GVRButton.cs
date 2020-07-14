using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GVRButton : MonoBehaviour
{
    public Image imgCircle;
    public UnityEvent GVRClick;
    public float totalTime = 2;
    bool gvrStatus;
    public float gvrTimer;

    private void Start()
    {
        imgCircle = GameObject.FindGameObjectWithTag("Gaze Reticle").GetComponent<Image>();
        imgCircle.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gvrStatus)
        {
            gvrTimer += Time.deltaTime;
            imgCircle.fillAmount = gvrTimer / totalTime;

            if (gvrTimer > totalTime)
            {
                GVRClick.Invoke();
                gvrStatus = false;
            }
        }
    }

    public void GVROn()
    {
        gvrStatus = true;
    }
    public void GvrOff()
    {
        gvrStatus = false;
        gvrTimer = 0;
        imgCircle.fillAmount = 0;
    }


}

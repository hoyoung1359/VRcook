using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class LeanTweenBasics : MonoBehaviour
{
    public LeanTweenType easeType1;
    public LeanTweenType easeType2;

    public GameObject uiElement1;
    public GameObject uiElement2;
    public GameObject uiElement3;

    public UnityEvent event1;
    public UnityEvent event2;


    void Update()
    {

        // scaling alternative ui panel
        if(Input.GetKeyDown(KeyCode.A))
        {
            LeanTween.scale(uiElement1, Vector3.zero, 0.5f).setEase(easeType1);
            LeanTween.scale(uiElement2, Vector3.one, 0.5f).setEase(easeType2);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            LeanTween.scale(uiElement1, Vector3.one, 0.5f).setEase(easeType2);
            LeanTween.scale(uiElement2, Vector3.zero, 0.5f).setEase(easeType1);
        }

        // scaling child gameobjects consequently
        if(Input.GetKeyDown(KeyCode.D))
        {
            event1.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            event2.Invoke();
        }
    }
}

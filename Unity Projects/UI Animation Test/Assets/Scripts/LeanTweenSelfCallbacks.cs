using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenSelfCallbacks : MonoBehaviour
{
    public void ScaleDownSelf()
    {
        LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEaseInBack();
    }

    public void ScaleUpSelf()
    {
        LeanTween.scale(gameObject, Vector3.one, 0.5f).setEaseOutBack();
    }
}

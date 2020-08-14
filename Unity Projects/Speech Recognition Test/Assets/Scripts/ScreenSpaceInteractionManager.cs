using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpaceInteractionManager : MonoBehaviour
{
    public ScreenSpaceInteraction lastActiveInstance;

    public void DeactivateLastInteraction()
    {
        if(lastActiveInstance != null)
            lastActiveInstance.deactivate();
    }
}

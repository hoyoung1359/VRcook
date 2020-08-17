using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpaceInteraction : MonoBehaviour
{
    public void activate()
    {
        var worldSpaceCanvas = GameObject.FindGameObjectWithTag("WorldSpaceCanvas").GetComponent<WorldSpaceCanvasController>();
        worldSpaceCanvas.Reposition();

        if (CommandExecutor.screenSpaceInteractionManager.lastActiveInstance != null)
            CommandExecutor.screenSpaceInteractionManager.lastActiveInstance.deactivate();

        CommandExecutor.screenSpaceInteractionManager.lastActiveInstance = this;

        onActivate();
    }

    public void deactivate()
    {
        onDeactivate();

        if(CommandExecutor.screenSpaceInteractionManager.lastActiveInstance == this)
            CommandExecutor.screenSpaceInteractionManager.lastActiveInstance = null;
    }

    public virtual void onActivate()
    {
    }

    public virtual void onDeactivate()
    {
    }
}
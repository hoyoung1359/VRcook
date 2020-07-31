using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceCanvasController : MonoBehaviour
{
    public float distanceFromCamera;

    public void Reposition()
    {
        transform.position = Camera.main.transform.position + Camera.main.transform.forward * distanceFromCamera;
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}

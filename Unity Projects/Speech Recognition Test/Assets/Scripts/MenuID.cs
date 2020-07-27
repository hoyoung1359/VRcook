using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuID : MonoBehaviour
{
    public int id;

    public void getRecipe()
    {
        Debug.Log($"selected id: {id}");
    }
}

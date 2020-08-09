using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuID : MonoBehaviour
{
    public int id;

    public void OnClick()
    {
        Debug.Log($"Requesting cooking step with food id : {id}");
        GameObject.FindGameObjectWithTag("RecipeGuide").GetComponent<RecipeGuide>().getRecipe(id);
    }
}

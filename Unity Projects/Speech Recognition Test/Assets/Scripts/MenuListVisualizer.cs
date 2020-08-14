using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct MenuInfo
{
    public int id;
    public string name;
}

public class MenuListVisualizer : ScreenSpaceInteraction
{
    public GameObject menuPrefab;
    public Transform menuHolder;
    public GameObject menuListParent;
    private List<MenuInfo> menuList;
    public List<MenuInfo> MenuList
    {
        set
        {
            menuList = value;
        }
    }

    public override void onActivate()
    {
        // Create button instances
        for (int i = 0; i < menuList.Count; i++)
        {
            GameObject menu = Instantiate(menuPrefab, menuHolder);
            menu.GetComponentInChildren<Text>().text = menuList[i].name;
            menu.GetComponentInChildren<MenuID>().id = menuList[i].id;
        }

        LeanTween.scale(menuListParent, Vector3.one, 0.5f).setEaseInOutExpo();
    }

    public override void onDeactivate()
    {
        LeanTween.scale(menuListParent, Vector3.zero, 0.5f).setEaseInOutBack().setOnComplete(DeleteChildButtons);
    }

    private void DeleteChildButtons()
    {
        foreach (Transform child in menuHolder)
        {
            Destroy(child.gameObject);
        }
    }
}
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
    public GameObject createDeleteUI;
    private List<MenuInfo> menuList;

    public override void onActivate()
    {
        createDeleteUI.SetActive(true);
        LeanTween.scale(createDeleteUI, Vector3.one, 0.5f).setEaseInOutExpo();

        DeleteChildButtons(); // Delete existing buttons, so that duplicate command will not cause duplicate buttons
        for (int i = 0; i < menuList.Count; i++)
        {
            GameObject menu = Instantiate(menuPrefab, menuHolder);
            menu.GetComponentInChildren<Text>().text = menuList[i].name;
            menu.GetComponentInChildren<MenuID>().id = menuList[i].id;
        }

        foreach (var menu in menuList)
        {
            Debug.Log($"MenuVisualizer => {menu.id}:{menu.name}");
        }
    }

    public override void onDeactivate()
    {
        Debug.Log("Hiding menulist");
        LeanTween.scale(menuHolder.gameObject, Vector3.zero, 0.5f).setEaseInOutBack().setOnComplete(DeleteChildButtons);
    }

    public List<MenuInfo> MenuList
    {
        set
        {
            menuList = value;
        }
    }

    public void ShowMenuList(List<MenuInfo> menuList)
    {
        this.activate();
    }

    public void HideMenuList()
    {
        this.deactivate();
    }

    private void DeleteChildButtons()
    {
        foreach (Transform child in menuHolder)
        {
            Destroy(child.gameObject);
        }
    }
}
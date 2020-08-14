using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct MenuInfo
{
    public int id;
    public string name;
}

public class MenuListVisualizer : MonoBehaviour
{
    public GameObject menuPrefab;
    public Transform menuHolder;
    public GameObject createDeleteUI;

    public void ShowMenuList(List<MenuInfo> menuList)
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

    public void HideMenuList()
    {
        Debug.Log("Hiding menu list");
        LeanTween.scale(menuHolder.gameObject, Vector3.zero, 0.5f).setEaseInOutBack().setOnComplete(DeleteChildButtons);
    }

    private void DeleteChildButtons()
    {
        foreach (Transform child in menuHolder)
        {
            Destroy(child.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MenuInfo
{
    public int id;
    public string name;
}

public class MenuListVisualizer : MonoBehaviour
{
    public void ShowMenuList(List<MenuInfo> menuList)
    {
        // 명섭: menuList에 있는 name들로 UI 띄우기.
        //       아래 코드를 보면 메뉴마다 id랑 name이 있는데,
        //       name은 텍스트로 보여주면 되고 id는 그냥 UI마다 저장하기만 하면 됨.
        //       나중에 db에 그 요리의 조리법을 검색하려 하면 이름 대신 id를 사용하기 때문임
        //
        // 호영: 메뉴 UI마다 저장된 id를 선택하면 가져올 수 있게 하면 됨.
        //       그럼 내가 db에 요청 보내서 조리 과정 가져올 수 있음!
        //       이번에는 거기까지 다루지 않으니까 선택되면 id를 Debug.Log로
        //       보여줄 수만 있으면 성공이야(e.g. "선택된 음식의 ID: 12345")
        foreach (var menu in menuList)
        {
            Debug.Log($"MenuVisualizer => {menu.id}:{menu.name}");
        }
    }
}

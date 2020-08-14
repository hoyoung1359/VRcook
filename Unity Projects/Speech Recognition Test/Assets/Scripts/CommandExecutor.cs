using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityScript.Steps;

/// <summary>
/// Uses VoiceRecognizer to parse commands and execute them
/// Actual execution is delegated to appropriate components,
/// such as DatabaseRequest, TimerManager, etc...
/// </summary>
[RequireComponent(typeof(VoiceRecognizer))]
[RequireComponent(typeof(DatabaseRequest))]
[RequireComponent(typeof(TimerManager))]
[RequireComponent(typeof(ScreenSpaceInteractionManager))]
[RequireComponent(typeof(RecognitionVisualizer))]
[RequireComponent(typeof(MenuListVisualizer))]
public class CommandExecutor : MonoBehaviour
{
    public GameObject notifier;
    private NotificationVisualizer notificationVisualizer;

    private DatabaseRequest databaseRequest;
    private TimerManager timerManager;
    public static ScreenSpaceInteractionManager screenSpaceInteractionManager;
    private MenuListVisualizer menuListVisualizer;
    private WorldSpaceCanvasController worldSpaceCanvas;
    public TimerStartDelete timerStartDelete;

    private bool isWaitingTimerCommand = false;

    void Start() 
    {
        var recognizer = GetComponent<VoiceRecognizer>();
        recognizer.recognitionResultHandler += RecognitionResultHandler;

        databaseRequest = GetComponent<DatabaseRequest>();
        timerManager = GetComponent<TimerManager>();
        screenSpaceInteractionManager = GetComponent<ScreenSpaceInteractionManager>();
        menuListVisualizer = GetComponent<MenuListVisualizer>();

        notificationVisualizer = notifier.GetComponent<NotificationVisualizer>();
        worldSpaceCanvas = GameObject.FindGameObjectWithTag("WorldSpaceCanvas").GetComponent<WorldSpaceCanvasController>();

        /* 키워드 포함된 요리 이름 검색 잘 되나 테스트하는 코드
        databaseRequest.SelectMenuList("오므라이스", SelectMenuListCallback);
        //*/

        /* 타이머 삭제 테스트할때 매번 생성하지 않도록 하는 코드
        timerManager.StartTimer(30);
        timerManager.StartTimer(20);
        timerManager.StartTimer(10);
        timerManager.ShowCreateDeleteUI();
        //*/

        /* 조리 단계 잘 가져오나 테스트하는 코드
        databaseRequest.SelectCookingStep(1, TestCallback);
        //*/
    }

    /*
    private void TestCallback(Row[] result)
    {
        foreach(var row in result)
        {
            foreach(var column in row.columns)
            {
                Debug.Log($"{column.name}:{column.value}");
            }
        }
    }
    //*/

    private void RecognitionResultHandler(object sender, string result)
    {
        // 명령어: "타이머.", "Timer."
        if ((result.StartsWith("타이머") && result.Length == 4) || (result.StartsWith("Timer") && result.Length == 6))
        {
            timerStartDelete.activate();
        }

        // 명령어: "검색 {키워드}."
        if (result.StartsWith("검색"))
        {
            var keyword = result.Substring(3, result.Length - 4);
            databaseRequest.SelectMenuList(keyword, SelectMenuListCallback);
        }

        // 명령어: "취소."
        if(result.StartsWith("취소") && result.Length == 3)
        {
            screenSpaceInteractionManager.DeactivateLastInteraction();
        }
    }

    private void SelectMenuListCallback(Row[] result)
    {
        if(result == null)
        {
            notificationVisualizer.Notify("제시된 키워드가 포함된 요리가 없습니다.");

            return;
        }

        // Convert raw query result to a menu list
        List<MenuInfo> menuList = new List<MenuInfo>();
        foreach(var row in result)
        {
            var menu = new MenuInfo();
            foreach(var column in row.columns)
            {
                if(column.name.Equals("ID"))
                {
                    menu.id = int.Parse(column.value);
                }
                if(column.name.Equals("Name"))
                {
                    menu.name = column.value;
                }
            }
            menuList.Add(menu);
        }

        // Update data and show the list
        menuListVisualizer.MenuList = menuList;
        menuListVisualizer.activate();
    }
}
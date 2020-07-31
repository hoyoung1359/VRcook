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
[RequireComponent(typeof(RecognitionVisualizer))]
[RequireComponent(typeof(MenuListVisualizer))]
public class CommandExecutor : MonoBehaviour
{
    public GameObject notifier;
    private NotificationVisualizer notificationVisualizer;

    private DatabaseRequest databaseRequest;
    private TimerManager timerManager;
    private MenuListVisualizer menuListVisualizer;
    private WorldSpaceCanvasController worldSpaceCanvas;

    private bool isWaitingTimerCommand = false;
    private bool isCommandLocked = false;

    void Start() 
    {
        var recognizer = GetComponent<VoiceRecognizer>();
        recognizer.recognitionResultHandler += RecognitionResultHandler;

        databaseRequest = GetComponent<DatabaseRequest>();
        timerManager = GetComponent<TimerManager>();
        menuListVisualizer = GetComponent<MenuListVisualizer>();

        notificationVisualizer = notifier.GetComponent<NotificationVisualizer>();
        worldSpaceCanvas = GameObject.FindGameObjectWithTag("WorldSpaceCanvas").GetComponent<WorldSpaceCanvasController>();

        /* 키워드 포함된 요리 이름 검색 잘 되나 테스트하는 코드
        databaseRequest.SelectMenuList("오므라이스", SelectMenuListCallback);
        //*/

        ///* 타이머 삭제 테스트할때 매번 생성하지 않도록 하는 코드
        timerManager.StartTimer(30);
        timerManager.StartTimer(20);
        timerManager.StartTimer(10);
        timerManager.ShowCreateDeleteUI();
        //*/
    }

    // Turns command executor into a waiting state,
    // which will then try to parse the consequent recognition
    // result as timer duration(e.g. "1시간 10분.").
    // Should be called only when timer start UI is selected.
    public void StartWaitingTimerCommand()
    {
        isWaitingTimerCommand = true;
    }

    private void RecognitionResultHandler(object sender, string result)
    {
        if(isWaitingTimerCommand)
        {
            isWaitingTimerCommand = false;
            try
            {
                timerManager.StartTimer(ParseTime(result));
            }
            catch (Exception e)
            {
                Debug.Log($"Failed to parse timer command('{result}') with error message: '{e.Message}'");
                notificationVisualizer.Notify("시간 해석에 실패했습니다. '타이머' 명령어를 다시 시도해주세요.");
            }
        }
        else
        {
            // 명령어: "타이머.", "Timer."
            if ((result.StartsWith("타이머") && result.Length == 4) || (result.StartsWith("Timer") && result.Length == 6))
            {
                worldSpaceCanvas.Reposition();
                timerManager.ShowCreateDeleteUI();
            }

            // 명령어: "검색 {키워드}."
            if (result.StartsWith("검색"))
            {
                var keyword = result.Substring(3, result.Length - 4);
                databaseRequest.SelectMenuList(keyword, SelectMenuListCallback);
            }
        }
    }

    private void SelectMenuListCallback(Row[] result)
    {
        if(result == null)
        {
            notificationVisualizer.Notify("제시된 키워드가 포함된 요리가 없습니다.");

            return;
        }

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

        worldSpaceCanvas.Reposition();
        menuListVisualizer.ShowMenuList(menuList);
    }

    // Calculates total duration of timer command in seconds
    // The format for the command is "[x시간] [x분] [x초]."
    // where brackets imply conditional arguments
    // Any punctuation mark at the end is ignored
    //
    // Ex) "일분 30초." => 90
    //     "1시간!" => 3600
    private int ParseTime(string timerCommand)
    {
        var timeResult = timerCommand.Substring(0, timerCommand.Length - 1); // Remove punctuation mark
        var timeList = timeResult.Split(' ');
        var time = 0;

        foreach (var token in timeList)
        {
            if (token.EndsWith("시간"))
            {
                var numericalPart = token.Substring(0, token.Length - 2);
                var parseSucceeded = int.TryParse(numericalPart, out int hour);

                if(!parseSucceeded)
                {
                    hour = TryParseKoreanNumber(numericalPart);
                }

                time += hour * 3600;
            }
            else if (token.EndsWith("분"))
            {
                var numericalPart = token.Substring(0, token.Length - 1);
                var parseSucceeded = int.TryParse(numericalPart, out int minute);

                if (!parseSucceeded)
                {
                    minute = TryParseKoreanNumber(numericalPart);
                }

                time += minute * 60;
            }
            else if (token.EndsWith("초"))
            {
                var numericalPart = token.Substring(0, token.Length - 1);
                var parseSucceeded = int.TryParse(numericalPart, out int seconds);

                if (!parseSucceeded)
                {
                    seconds = TryParseKoreanNumber(numericalPart);
                }

                time += seconds;
            }
            else
            {
                throw new Exception($"unexpected token in timer command: {token}");
            }
        }

        Debug.Log($"Parsed time in seconds: {time}");

        return time;
    }

    private int KoreanCharToInt(char korean)
    {
        char[] koreanInt = { ' ', '일', '이', '삼', '사', '오', '육', '칠', '팔', '구', '십' };
        if (Array.Exists(koreanInt, element => element == korean))
        {
            return Array.IndexOf(koreanInt, korean);
        }
        else if (korean == '백')
        {
            return 100;
        }
        throw new NotImplementedException("failed to convert numbers in korean into an integer(e.g. 십이 => 12");
    }

    // Try to convert a given string, possibly in koreaninto an integer(e.g. "십이" => 12).
    // Throws an exception when failed.
    private int TryParseKoreanNumber(string numericalPart)
    {
        Debug.Log($"Trying to parse \"{numericalPart}\" as an integer...");

        int result = 0;
        try
        {
            result = int.Parse(numericalPart);
        }
        catch
        {
            while (numericalPart.Length > 0)
            {
                if ((numericalPart[0] == '백' || numericalPart[0] == '십') && numericalPart.Length > 1)
                {
                    result += KoreanCharToInt(numericalPart[0]);
                    numericalPart = numericalPart.Substring(1);
                }
                else if (numericalPart.Length > 2)
                {
                    result += KoreanCharToInt(numericalPart[0]) * KoreanCharToInt(numericalPart[1]);
                    numericalPart = numericalPart.Substring(2);
                }
                else if (numericalPart.Length == 2)
                {
                    result += KoreanCharToInt(numericalPart[0]) * KoreanCharToInt(numericalPart[1]);
                    numericalPart = "";
                }
                else
                {
                    result += KoreanCharToInt(numericalPart[0]);
                    numericalPart = "";
                }
            }
        }
        return result;
    }
}
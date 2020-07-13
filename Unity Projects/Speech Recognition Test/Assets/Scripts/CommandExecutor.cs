using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private DatabaseRequest databaseRequest;
    private TimerManager timerManager;
    private MenuListVisualizer menuListVisualizer;

    private bool isWaitingTimerCommand = false;

    void Start()
    {
        var recognizer = GetComponent<VoiceRecognizer>();
        recognizer.recognitionResultHandler += RecognitionResultHandler;

        databaseRequest = GetComponent<DatabaseRequest>();
        timerManager = GetComponent<TimerManager>();
        menuListVisualizer = GetComponent<MenuListVisualizer>();

        /* 키워드 포함된 요리 이름 검색 잘 되나 테스트하는 코드
        databaseRequest.SelectMenuList("오므라이스", SelectMenuListCallback);
        */
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
            try
            {
                timerManager.StartTimer(ParseTime(result));
            }
            catch (Exception e)
            {
                Debug.Log($"Failed to parse timer command('{result}') with error message: '{e.Message}'");

                // 파싱 실패하면 타이머 명령어 대기 취소
                isWaitingTimerCommand = false;
            }
        }
        else
        {
            // 명령어: "타이머."
            if (result.StartsWith("타이머") && result.Length == 4)
            {
                timerManager.ShowCreateDeleteUI();
            }

            /* db 쿼리 잘 되나 테스트하는 코드
            if (result.StartsWith("요청"))
            {
                Debug.Log("Starting request");
                databaseRequest.Select("testTable", SelectCallback);
            }
            */

            // 명령어: "검색 {키워드}."
            if (result.StartsWith("검색"))
            {
                var keyword = result.Substring(2);
                databaseRequest.SelectMenuList(keyword, SelectMenuListCallback);
            }
        }
    }

    /* 쿼리 결과 테이블을 전부 보여주는 테스트 코드
    private void SelectCallback(Row[] result)
    {
        Debug.Log("Select callback");
        for(var rowIndex = 0; rowIndex < result.Length; rowIndex++)
        {
            Debug.Log($"Row {rowIndex}");
            foreach(var column in result[rowIndex].columns)
            {
                Debug.Log($"{column.name}:{column.value}");
            }
        }
    }
    */

    private void SelectMenuListCallback(Row[] result)
    {
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
        // 상헌: 한글로 된 숫자 파싱하기.
        //       지금 시,분,초 3부분에서 다 같은 작업을 요구한다는건
        //       한글->숫자 변환 작업을 함수로 분리해야 한다는 의미야
        //       throw new NotImplementedException()이 있는 부분마다 그 작업이 필요하니까
        //       ParseKoreanNumber()같은 함수 하나 만들어서 처리해주면 좋겠어
        //       


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

    // Try to convert a given string, possibly in koreaninto an integer(e.g. "십이" => 12).
    // Throws an exception when failed.
    private int TryParseKoreanNumber(string numericalPart)
    {
        // 상헌: 한글로 된 숫자를 정수로 변환하면 됨
        //       혹시 숫자가 아니거나 모종의 이유로 실패하면 그냥 아래처럼 excepion을 아무거나 던져줘
        //       그럼 명령어 처리하는 함수에서 타이머를 실행하는 대신 catch 구문으로 들어갈거야
        Debug.Log($"Trying to parse \"{numericalPart}\" as an integer...");
        throw new NotImplementedException("failed to convert numbers in korean into an integer(e.g. 십이 => 12");
    }
}

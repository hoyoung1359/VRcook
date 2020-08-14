using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDurationListener : ScreenSpaceInteraction
{
    public VoiceRecognizer voiceRecognizer;
    public TimerManager timerManager;
    public NotificationVisualizer notificationVisualizer;

    public override void onActivate()
    {
        voiceRecognizer.recognitionResultHandler += TryStartTimer;
    }

    public override void onDeactivate()
    {
        voiceRecognizer.recognitionResultHandler -= TryStartTimer;
    }

    private void TryStartTimer(object sender, string result)
    {
        try
        {
            timerManager.StartTimer(ParseTime(result));
        }
        catch (Exception e)
        {
            Debug.Log($"Failed to parse timer command('{result}') with error message: '{e.Message}'");
            notificationVisualizer.Notify("시간 해석에 실패했습니다. '타이머' 명령어를 다시 시도해주세요.");
        }

        deactivate();
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

                if (!parseSucceeded)
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

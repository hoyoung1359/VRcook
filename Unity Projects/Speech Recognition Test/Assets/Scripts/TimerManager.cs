using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private List<Timer> timers;

    void Start()
    {
        timers = new List<Timer>();
    }

    void Update()
    {
        foreach(var timer in timers)
        {
            if(timer.Progress(Time.deltaTime))
            {
                OnTimerFinish();
            }
            else
            {
                // 화면 우측 상단에 지금 돌아가는 타이머 목록을 보여줘야 함
                // 시계 아이콘 옆에 진행도 막대기랑 남은 시간(00:01:40) 문자열 보여주자
            }
        }
    }

    public void StartTimer(float seconds)
    {
        // Timer라는 객체를 만들어서 timers 리스트에 넣자
        // 생성자에 seconds를 넣어주면 된다
    }

    private void OnTimerFinish()
    {
        // 타이머 끝나고 나서 아이콘을 바로 지워버리면 알아채기가 힘드니까 좀 놔뒀다가 사용자가 확인하면 삭제하는
        // 방식으로 해야 하는데 이것도 세부적으로 어떻게 처리할지 정해야 함
        //
        // Ex1) "타이머 1번 중지"같은 명령어로 알림 소리 끄고 아이콘 삭제
        // Ex2) 끝난 타이머 아이콘 3초 바라보면 삭제
        //
        // 일단 밑에 적어놨듯이 아이콘을 좀 눈에 띄게 바꾸고 알림 소리 내면 좋을듯
    }
}

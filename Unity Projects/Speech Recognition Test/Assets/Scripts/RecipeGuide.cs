using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public struct CookingStep
{
    public int id;
    public string description;
}

public class RecipeGuide : MonoBehaviour
{

    private int nextStep = 0;
    private List<CookingStep> recipe;
    private NotificationVisualizer notificationVisualizer;

    void Start()
    {
        recipe = new List<CookingStep>();
        notificationVisualizer = GameObject.FindGameObjectWithTag("Notifier").GetComponent<NotificationVisualizer>();
    }

    void PrepareRecipe(Row[] result)
    {
        if (result == null)
        {
            notificationVisualizer.Notify("찾으려는 요리의 조리법이 없었습니다. 관리자에게 문의해주세요.");

            return;
        }


         for (int i = 0; i < result.Length; i++)
        {
        
         for (int j= 0; j < result[i].columns.Length; j++) {
         CookingStep nstep = new CookingStep();

             if (result[i].columns[j].name == "CookingStepID") {
                var id = result[i].columns[j].value;
                nstep.id = Convert.ToInt32(id);
              }
             if (result[i].columns[j].name == "Description") { 
                var description = result[i].columns[j].value;
                nstep.description = description;
              }

          recipe.Add(nstep);
         }
        
    }



        // result 안에 cooking step의 id랑 description이 들어있어
        // 그걸 이 클래스의 멤버 변수 recipe에 차례대로 넣으면 돼
        // 여기 넣은 정보는 밑에 있는 StartNextStep()에서 사용할거야
        Debug.Log("Creating recipe instance from database query result");
}

    public void StartNextStep()
    {
        if (nextStep >= recipe.Count)
        {
            notificationVisualizer.Notify("남아있는 조리 단계가 없습니다. 프로그램을 종료하거나 새로운 요리를 시작해주세요.");

            return;
        }

        var currentCookingStep = recipe[nextStep++];


        // 여기서 currentCookingStep의 id로 어떤 애니메이션을 띄울지 알아내고
        // description으로 화면에 설명을 보여줄거야
        // 그건 UI랑 애니메이션 모두 있어야 하니까 일단 로그만 남기도록 했어
        // 이 함수는 사용자가 음성 명령으로 "다음"이라고 했을 때마다 호출되게 할거야
        Debug.Log($"Currently showing cooking step with id: {currentCookingStep.id}, description: {currentCookingStep.description}");
    }


    public void getRecipe()
    {
        MenuID mid = new MenuID();
        GameObject.Find("VoiceRecognizer").GetComponent<DatabaseRequest>().SelectCookingStep(mid.id, PrepareRecipe);
        
        Debug.Log($"selected id: {mid.id}");
    }
}

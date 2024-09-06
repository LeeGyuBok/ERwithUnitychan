using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldQuestContent : QuestContent
{
    public static QuestContent goldQuest { get; }= new GoldQuestContent();

    private GoldQuestContent()
    {
        Title = "금 구하기";
        Reward = "친밀도(중)";
        Objective = ItemPool.Instance.itemArray[5].KoreanName;
        ObjectiveGoalCount = 6;
        QuestStringContent = "여기있는 녀석들의 대부분은 머리가 이상해. 금을 여기저기 뿌리고 다닌다니까? 돈 아까운 줄 모른단 말이지? 그래서 말인데, 그 금 좀 가져와라.";
    }
}

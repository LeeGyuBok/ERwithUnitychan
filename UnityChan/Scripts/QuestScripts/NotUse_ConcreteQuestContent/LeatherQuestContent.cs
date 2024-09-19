using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeatherQuestContent : QuestContent
{
    public static QuestContent LeatherQuest { get; }= new LeatherQuestContent();

    private LeatherQuestContent()
    {
        Title = "가죽 구하기";
        Reward = "친밀도(소)";
        Objective = "가죽";
        ObjectiveGoalCount = 3;
        QuestStringContent =
            "너네가 만든 동물들한테서 나오는 가죽말이야, 엄청 튼튼하던데, 나한테 좀 구해줘. 섬에 들어가는 순간 일단 살아남는게 먼저라 그런 걸 가만히 들고 있을 시간은 없거든.";
    }
}

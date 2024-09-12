using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBySo : MonoBehaviour
{
    public Quest_SO questData;

    public void GetQuestData()
    {
        Debug.Log(questData.Title);
        Debug.Log(questData.Reward);
        Debug.Log(questData.Target);
        Debug.Log(questData.TargetGoalCount);
    }
}

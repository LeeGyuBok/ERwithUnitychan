using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest", fileName = "QuestContent")]
public class Quest_SO : ScriptableObject
{
    //퀘스트 제목
    [SerializeField]private string title;
    public string Title
    {
        get { return title; }
    }
    //퀘스트 보상
    [SerializeField]private string reward;
    public string Reward
    {
        get { return reward; }
    }
    
    //퀘스트 목표 아이템
    [SerializeField]private string target;
    public string Target
    {
        get { return target; }
    }
    //퀘스트 목표 아이템 요구 개 수
    [SerializeField]private int targetGoalCount;
    public int TargetGoalCount
    {
        get { return targetGoalCount; }
    }
}

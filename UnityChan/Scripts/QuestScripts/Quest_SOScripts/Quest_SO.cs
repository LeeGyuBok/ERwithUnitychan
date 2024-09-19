using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Quest", fileName = "QuestContent")]
public class Quest_SO : ScriptableObject
{
    //NPC 코드, Resources/NPC에서 이미지 순서 및 이미지의 번호 확인할 것, Nadja는 사용 x
    [SerializeField]private int npcCode;
    public int NpcCode
    {
        get { return npcCode; }
    }
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
    
    //퀘스트 목표 아이템의 아이템 코드
    [SerializeField]private int itemCode;
    public int ItemCode
    {
        get { return itemCode; }
    }
    //퀘스트 목표 아이템 요구 개 수
    [SerializeField]private int targetGoalCount;
    public int TargetGoalCount
    {
        get { return targetGoalCount; }
    }
    [SerializeField]private string questDetail;
    public string QuestDetail
    {
        get { return questDetail; }
    }
    
}

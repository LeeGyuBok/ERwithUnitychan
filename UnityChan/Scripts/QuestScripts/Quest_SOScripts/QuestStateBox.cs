using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Quest {Gold, Leather}

public class QuestStateBox : MonoBehaviour
{
    public static QuestStateBox Instance;
    
    [SerializeField] private List<Quest_SO> questDatas;
    public List<QuestContent_SO> ContinueQuestList_SO { get; private set; }
    public List<QuestContent_SO> CompleteQuestList_SO { get; private set; }
    
    public List<QuestContent_SO> DeclineQuestList_SO { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            DeclineQuestList_SO = new List<QuestContent_SO>();
            ContinueQuestList_SO = new List<QuestContent_SO>();
            CompleteQuestList_SO = new List<QuestContent_SO>();
        }
        else
        {
            Debug.Log("Critical Error: QuestStateBox");
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeDeclineQuestList();
        
        foreach (var quest in DeclineQuestList_SO)
        {
            if (quest != null)
            {
                QuestWindow_Machine.Instance.QuestListUpdate(quest);        
            }
            else
            {
                return;
            }
        }
    }
    

    // 퀘스트 상태에 따른 미수락 퀘스트리스트 업데이트하기
    public void UpdateDeclineQuestList()
    {
        // 일시적인 리스트 생성, 저장하기 위한, 미수락 퀘스트리스트에서 제거할 퀘스트들의 - 참조 제거
        List<QuestContent_SO> questsToRemove = new List<QuestContent_SO>();
        
        // 미수락 퀘스트리스트 내부 순회
        foreach (QuestContent_SO quest in DeclineQuestList_SO)
        {
            switch (quest.Status)
            {
                //상태가 수락 상태로 변하면
                case QuestStatus.Continue:
                    ContinueQuestList_SO.Add(quest); // ContinueQuestList에 넣고
                    questsToRemove.Add(quest);    // 지울 리스트(일시적)에 넣고
                    break;
                //상태가 완료 상태로 변하면
                case QuestStatus.Complete:
                    CompleteQuestList_SO.Add(quest); // CompleteQuestList에 넣고
                    questsToRemove.Add(quest);    // 지울 리스트(일시적)에 넣고
                    break;
                //위의 두상태가 아니고, 기본 상태도 아니면
                default:
                    if (!quest.Status.Equals(QuestStatus.Decline))
                    {
                        Debug.LogWarning($"Quest has an unknown status: {quest.Status.ToString()}");    
                    }
                    break;
                //여전히 미수락 상태면 내버려둠
            }
        }
        // 옮겨진 퀘스트들을 지운다.
        foreach (QuestContent_SO quest in questsToRemove)
        {
            DeclineQuestList_SO.Remove(quest);
        }
    }
    // 퀘스트 상태에 따른 수락 퀘스트리스트 업데이트하기
    public void UpdateContinueQuestList()
    {
        // 일시적인 리스트 생성, 저장하기 위한, 미수락 퀘스트리스트에서 제거할 퀘스트들의 - 참조 제거
        List<QuestContent_SO> questsToRemove = new List<QuestContent_SO>();
        
        // 미수락 퀘스트리스트 내부 순회
        foreach (QuestContent_SO quest in ContinueQuestList_SO)
        {
            switch (quest.Status)
            {
                //상태가 거절(포기) 상태로 변하면
                case QuestStatus.Decline:
                    DeclineQuestList_SO.Add(quest); // DeclineQuestList에 넣고
                    questsToRemove.Add(quest);    // 지울 리스트(일시적)에 넣고
                    break;
                // 상태가 완료 상태로 변하면
                case QuestStatus.Complete:
                    CompleteQuestList_SO.Add(quest); // CompleteQuestList에 넣고
                    questsToRemove.Add(quest);    // 지울 리스트(일시적)에 넣고
                    break;
                //위의 두상태가 아니고, 수락 상태도 아니면
                default:
                    if (!quest.Status.Equals(QuestStatus.Continue))
                    {
                        Debug.LogWarning($"Quest has an unknown status: {quest.Status.ToString()}");    
                    }
                    break;
                //여전히 미수락 상태면 내버려둠
            }
        }
        // 옮겨진 퀘스트들을 지운다.
        foreach (QuestContent_SO quest in questsToRemove)
        {
            ContinueQuestList_SO.Remove(quest);
        }
    }

    
    //여기가 퀘스트리스트업데이트하는 곳
    private void InitializeDeclineQuestList()
    {
        /*DeclineQuestList.Add(new Quest_My(NpcPool.Instance.GetNpc("Hyunwoo"), QuestContentPool.Instance.QuestContents[0], false, false));
        DeclineQuestList.Add(new Quest_My(NpcPool.Instance.GetNpc("Darko"), QuestContentPool.Instance.QuestContents[1], false, false));*/

        for (int i = 0; i < questDatas.Count; i++)
        {
            Debug.Log(questDatas.Count);
            QuestContent_SO quest = new QuestContent_SO(questDatas[i], NpcPool.Instance.GetNpc(questDatas[i].NpcCode));
            DeclineQuestList_SO.Add(quest);
            Debug.Log(quest.Contents.Title);
            Debug.Log(quest.Contents.ItemCode);
            Debug.Log(quest.Contents.TargetGoalCount);
            Debug.Log(quest.Contents.Reward);
            Debug.Log($"내용: {quest.Contents.QuestDetail}");
        }
    }
}

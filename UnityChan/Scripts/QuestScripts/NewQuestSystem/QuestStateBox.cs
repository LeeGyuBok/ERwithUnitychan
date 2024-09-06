using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStateBox : MonoBehaviour
{
    public static QuestStateBox Instance;
    
    
    public List<Quest_My> DeclineQuestList { get; private set; }
    public List<Quest_My> ContinueQuestList { get; private set; }
    public List<Quest_My> CompleteQuestList { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            DeclineQuestList = new List<Quest_My>();
            ContinueQuestList = new List<Quest_My>();
            CompleteQuestList = new List<Quest_My>();
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
        DeclineQuestList.Add(new Quest_My(NpcPool.Instance.ShowNpc("Hyunwoo"), QuestContentPool.Instance.QuestContents[0], false, false));
        DeclineQuestList.Add(new Quest_My(NpcPool.Instance.ShowNpc("Darko"), QuestContentPool.Instance.QuestContents[1], false, false));
        
        foreach (var quest in DeclineQuestList)
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

    /*// Update is called once per frame
    void Update()
    {
        
    }*/
    

    // 퀘스트 상태에 따른 미수락 퀘스트리스트 업데이트하기
    public void UpdateDeclineQuestList()
    {
        // 일시적인 리스트 생성, 저장하기 위한, 미수락 퀘스트리스트에서 제거할 퀘스트들의 - 참조 제거
        List<Quest_My> questsToRemove = new List<Quest_My>();
        
        // 미수락 퀘스트리스트 내부 순회
        foreach (Quest_My quest in DeclineQuestList)
        {
            switch (quest.Status)
            {
                //상태가 수락 상태로 변하면
                case QuestStatus.Continue:
                    ContinueQuestList.Add(quest); // ContinueQuestList에 넣고
                    questsToRemove.Add(quest);    // 지울 리스트(일시적)에 넣고
                    break;
                //상태가 완료 상태로 변하면
                case QuestStatus.Complete:
                    CompleteQuestList.Add(quest); // CompleteQuestList에 넣고
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
        foreach (Quest_My quest in questsToRemove)
        {
            DeclineQuestList.Remove(quest);
        }
    }
    // 퀘스트 상태에 따른 수락 퀘스트리스트 업데이트하기
    public void UpdateContinueQuestList()
    {
        // 일시적인 리스트 생성, 저장하기 위한, 미수락 퀘스트리스트에서 제거할 퀘스트들의 - 참조 제거
        List<Quest_My> questsToRemove = new List<Quest_My>();
        
        // 미수락 퀘스트리스트 내부 순회
        foreach (Quest_My quest in ContinueQuestList)
        {
            switch (quest.Status)
            {
                //상태가 거절(포기) 상태로 변하면
                case QuestStatus.Decline:
                    DeclineQuestList.Add(quest); // DeclineQuestList에 넣고
                    questsToRemove.Add(quest);    // 지울 리스트(일시적)에 넣고
                    break;
                // 상태가 완료 상태로 변하면
                case QuestStatus.Complete:
                    CompleteQuestList.Add(quest); // CompleteQuestList에 넣고
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
        foreach (Quest_My quest in questsToRemove)
        {
            ContinueQuestList.Remove(quest);
        }
    }
}

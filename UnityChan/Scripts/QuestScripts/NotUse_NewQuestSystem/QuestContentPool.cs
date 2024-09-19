using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestContentPool : MonoBehaviour
{
    public static QuestContentPool Instance;

    public QuestContent[] QuestContents { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            QuestContents = new QuestContent[10];
            UpdateQuestContents();
        }
        else
        {
            Debug.Log("Critical Error: QuestContentPool");
            Destroy(gameObject);
        }
    }

    private void UpdateQuestContents()
    {
        //여기서걸림
        QuestContent leatherQuest = LeatherQuestContent.LeatherQuest;
        QuestContent goldQuest = GoldQuestContent.goldQuest;
        QuestContents[0] = leatherQuest;
        QuestContents[1] = goldQuest;
    }
}

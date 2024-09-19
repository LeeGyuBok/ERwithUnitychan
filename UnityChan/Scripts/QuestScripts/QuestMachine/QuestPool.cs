using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPool : MonoBehaviour
{
    public static QuestPool Instance;
    
    public Quest_My[] possibleQuestArray { get; private set; }

    private int capacity = 100;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            possibleQuestArray = new Quest_My[capacity];
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Critical Error: QuestPool");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        possibleQuestArray[0] = new Quest_My(NpcPool.Instance.GetNpc(3), QuestContentPool.Instance.QuestContents[0], false, false);
        possibleQuestArray[1] = new Quest_My(NpcPool.Instance.GetNpc(74), QuestContentPool.Instance.QuestContents[1], false, false);
        /*Debug.Log(possibleQuestArray[0].NPC.Name);//잘 나옴*/
        /*foreach (var quest in possibleQuestArray)
        {
            if (quest != null)
            {
                QuestWindow_Machine.instance.PlayerQuestListContinueUpdate(quest);        
            }
            else
            {
                return;
            }
        }*/
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

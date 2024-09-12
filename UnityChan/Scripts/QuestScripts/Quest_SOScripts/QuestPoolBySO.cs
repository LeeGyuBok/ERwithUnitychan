using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.HighDefinition;
using UnityEngine;

public enum Quest {Gold, Leather}
public class QuestPoolBySO : MonoBehaviour
{
    [SerializeField] private List<Quest_SO> questDatas;
    [SerializeField] private GameObject questSOPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < questDatas.Count; i++)
        {
            QuestBySo quest = QuestUpdate((Quest)i);
            quest.GetQuestData();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public QuestBySo QuestUpdate(Quest type)
    {
        var quest = Instantiate(questSOPrefab.GetComponent<QuestBySo>());
        quest.questData = questDatas[(int)type];
        return quest;
    }
}



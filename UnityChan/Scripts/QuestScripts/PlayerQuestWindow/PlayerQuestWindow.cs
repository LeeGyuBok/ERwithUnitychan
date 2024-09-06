using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerQuestWindow : MonoBehaviour
{
    [SerializeField] private GameObject questWindow;
    [SerializeField] private Button questSummaryButton;

    public static PlayerQuestWindow Instance;
    

    private List<Quest_My> questList;
    private Dictionary<Quest_My, Button> button_ByQuest;
    private LinkedList<Button> buttonLocation;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            questList = new List<Quest_My>(10);
            button_ByQuest = new Dictionary<Quest_My, Button>();
            buttonLocation = new LinkedList<Button>();
                
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (!questWindow.activeInHierarchy)
            {
                //수락한 퀘스트를 ui에 업데이트 하는 로직이 필요함
                questWindow.SetActive(true);
                return;
            }
            questWindow.SetActive(false);
        }
    }
    //퀘스트 머신으로부터 퀘스트를 수락하는 로직이 필요함.
    //퀘스트 머신은 퀘스트 스테이트박스에 있는 디클라인 리스트를 받아옴.
    //그럼 플레이어퀘스트윈도우는 수락한 퀘스트만 나타나야함. -> 컨티뉴퀘스트리스트
    public void PlayerQuestListContinueUpdate(Quest_My quest)
    {
        if (questList.Contains(quest))
        {
            Debug.Log("PlayerQuestWindow: Already Contains");
            return;
        }
        questList.Add(quest);
        //버튼을 생성하고 그 버튼의 하위 요소들에 텍스트를 넣는다.
        Button button = Instantiate(questSummaryButton, questWindow.transform, false);
        button.name = $"QuestSummaryButton{questList.Count}";
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(290, 30);
        rectTransform.anchoredPosition = new Vector2(0, 60 - 30 * questList.Count);
        //퀘스트머신의 디테일창을 열려고 하는데 어떻게 열수 있을까?
        /*Button buttonComponent = button.GetComponent<Button>();
        buttonComponent.onClick.AddListener(() => QuestWindow_Machine.Instance.ShowQuestDetail(button));*/
        //return button;
        button_ByQuest[quest] = button;
        if (buttonLocation.Last != null)
        {
            buttonLocation.AddAfter(buttonLocation.Last, button_ByQuest[quest]);
        }
        else
        {
            buttonLocation.AddFirst(button_ByQuest[quest]);
        }
        SummaryButtonInfoUpdate(quest);
    }

    public void PlayerQuestListDeclineUpdate(Quest_My quest)
    {
        if (button_ByQuest[quest] != null)
        {
            //딕셔너리에 해당 퀘스트의 버튼이 있다면 그 버튼을 지운다
            Destroy(button_ByQuest[quest].gameObject);
            if (questList.Contains(quest))
            {
                //퀘스트 삭제
                questList.Remove(quest);
                //버튼 지우고 다시 링크
                buttonLocation.Remove(button_ByQuest[quest]);
                //여기서 지워진 버튼의 위치에 대한 정렬 들어감
                ButtonLocationUpdate();
            }
        }
    }

    private void SummaryButtonInfoUpdate(Quest_My quest)
    {
        if (button_ByQuest[quest] != null)
        {
            Button targetButton = button_ByQuest[quest];
            GameObject title = targetButton.transform.Find("Title/TXT").gameObject;
            if (title.TryGetComponent(out TextMeshProUGUI titleTxt))
            {
                titleTxt.text = quest.QuestContents.Title;
            }

            GameObject objective = targetButton.transform.Find("TargetItem/TXT").gameObject;
            if (objective.TryGetComponent(out TextMeshProUGUI objectiveTxt))
            {
                objectiveTxt.text = quest.QuestContents.Objective;
            }

            GameObject count = targetButton.transform.Find("Count/TXT").gameObject;
            if (count.TryGetComponent(out TextMeshProUGUI countTxt))
            {
                countTxt.text = $"{quest.QuestContents.PlayerCollectCount} / {quest.QuestContents.ObjectiveGoalCount}";
            }
        }
    }

    private void ButtonLocationUpdate()
    {
        //버튼의 위치를 어떻게 조정하나? 가장 첫번째 버튼 위치는 (0, 30) 이후 (0, 30씩) 감소하며(y좌표 -30) 배치
        //0,30 0,0 0,-30
        //각 버튼의 y좌표를 추출한다.
        int index = 1;
        foreach (Button button in buttonLocation)
        {
            if (button.TryGetComponent(out RectTransform rectTransform))
            {
                float newYPosition = 60 - 30 * index;
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newYPosition);
            }
            index++;
        }
    }

    public void QuestStatusCheck()
    {
        //퀘스트의 진행상황을 확인한다. 어떻게 확인할까?
        //일단 퀘스트는 아이템 수집이다. -> 아이템을 획득할 때 체크한다.
        //인벤토리를 순회하면서 아이템이름을 퀘스트리스트의 오브젝티브와 비교한다.
        
        //인벤토리 크기만큼 순회할건데
        for (int i = 0; i < Inventory_Player.Instance.InventoryData.Length; i++)
        {
            //인벤토리내의 데이터관리하는 배열(스택들로 이루어짐)에서 아이템 가지고옴(Peek)
            Item_My item = Inventory_Player.Instance.InventoryData[i].Peek();
            //가지고온 아이템이 빈칸이거나 막힘이 아니면
            if (!item.ItemId.Equals(EnumItemCode.Blank.ToString()) || !item.ItemId.Equals(EnumItemCode.Blocked.ToString()))
            {
                //내가 수락한 퀘스트리스트의 퀘스트로 가서
                foreach (Quest_My quest in questList)
                {
                    //그 퀘스트의 오브젝티브와 비교한다.
                    if (quest.QuestContents.Objective.Equals(item.KoreanName))//만약 같은 아이템이면
                    {
                        //지역변수생성해서 그 아이템의 개수 넣고
                        int itemEA = Inventory_Player.Instance.InventoryData[i].Count;
                        
                        //플레이어가 모은 아이템 개수에 넣는다.
                        quest.QuestContents.PlayerCollectCount = itemEA;
                        
                        //퀘스트창 업데이트 준비
                        GameObject count = button_ByQuest[quest].transform.Find("Count/TXT").gameObject;
                        if (count.TryGetComponent(out TextMeshProUGUI countTxt))
                        {
                            //만약, 플레이어가 모은 아이템 개수가 퀘스트가 요구하는 개수보다 적으면
                            if (itemEA < quest.QuestContents.ObjectiveGoalCount)
                            {
                                countTxt.text = $"{quest.QuestContents.PlayerCollectCount} / {quest.QuestContents.ObjectiveGoalCount}";
                            }
                            else
                            {
                                countTxt.text = $"{quest.QuestContents.ObjectiveGoalCount} / {quest.QuestContents.ObjectiveGoalCount}";
                            }
                        }
                    }
                }
            }
            return;
        }
    }
}

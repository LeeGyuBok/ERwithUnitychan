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

    private static PlayerQuestWindow instance;

    public static PlayerQuestWindow Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerQuestWindow>();
                if (instance == null)
                {
                    GameObject singletonObj = new GameObject("PlayerQuestWindow");
                    instance = singletonObj.AddComponent<PlayerQuestWindow>();
                }
                
                DontDestroyOnLoad(instance);
            }

            return instance;
        }
        
    }
    

    public List<QuestContent_SO> questList { get; private set; }
    private Dictionary<QuestContent_SO, Button> button_ByQuest;
    private LinkedList<Button> buttonLocation;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        questList = new List<QuestContent_SO>(10);
        buttonLocation = new LinkedList<Button>();
        button_ByQuest = new Dictionary<QuestContent_SO, Button>();
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
    public void PlayerQuestListContinueUpdate(QuestContent_SO quest)
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

    public void PlayerQuestListDeclineUpdate(QuestContent_SO quest)
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

    private void SummaryButtonInfoUpdate(QuestContent_SO quest)
    {
        if (button_ByQuest[quest] != null)
        {
            Button targetButton = button_ByQuest[quest];
            GameObject title = targetButton.transform.Find("Title/TXT").gameObject;
            if (title.TryGetComponent(out TextMeshProUGUI titleTxt))
            {
                titleTxt.text = quest.Contents.Title;
            }

            GameObject target = targetButton.transform.Find("TargetItem/TXT").gameObject;
            if (target.TryGetComponent(out TextMeshProUGUI objectiveTxt))
            {
                Item_SO targetItem = ItemManager_SO.Instance.GetItem(quest.Contents.ItemCode);
                objectiveTxt.text = targetItem.data.KoreanName;
            }

            GameObject count = targetButton.transform.Find("Count/TXT").gameObject;
            if (count.TryGetComponent(out TextMeshProUGUI countTxt))
            {
                countTxt.text = $"{quest.PlayerCollectCount} / {quest.Contents.TargetGoalCount}";
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
        
        //퀘스트리스트(수락한 퀘스트들)의 각각의 퀘스트에 대해서
        foreach (QuestContent_SO quest in questList)
        {
            //인벤토리 크기만큼 순회할건데
            foreach (var inventoryItem in Inventory_Player.Instance.InventoryData)
            {
                Item_SO item = inventoryItem.Peek();
                //인벤토리의 아이템이 퀘스트가 요구하는 아이템과 같다면
                if (quest.Contents.ItemCode.ToString().Equals(item.data.ItemID))
                {
                    GameObject count = button_ByQuest[quest].transform.Find("Count/TXT").gameObject;
                    if (count.TryGetComponent(out TextMeshProUGUI countTxt))
                    {
                        quest.PlayerCollectCount = inventoryItem.Count;
                        if (item.data.MaxQuantity == 1)
                        {
                            int onlyOneItemTotalCount = 0;
                            //여기서 재순회하므로 0으로 친다 ㅋㅋ;
                            foreach (var inventoryItemOnlyOne in Inventory_Player.Instance.InventoryData)
                            {
                                if (item.data.ItemID.Equals(inventoryItemOnlyOne.Peek().data.ItemID))//현재 item에서 멈췄으니
                                {
                                    onlyOneItemTotalCount++;
                                }
                            }
                            countTxt.text = $"{onlyOneItemTotalCount} / {quest.Contents.TargetGoalCount}";
                        }
                        else
                        {
                            countTxt.text = $"{quest.PlayerCollectCount} / {quest.Contents.TargetGoalCount}";    
                        }
                        
                        
                    }
                    //완료여부를 체크한다.
                    CompleteQuest(quest, inventoryItem.Count);
                }
            }
        }
        
        
        /*//폐기코드
         * //인벤토리 크기만큼 순회할건데
        foreach (var inventoryItem in Inventory_Player.Instance.InventoryData)
        {
            //인벤토리내의 데이터관리하는 배열(스택들로 이루어짐)에서 아이템 가지고옴(Peek)
            Item_SO item = inventoryItem.Peek();
            //가지고온 아이템이 빈칸이거나 막힘이 아니면
            if (!item.data.ItemID.Equals(EnumItemCode.Blank.ToString()) || !item.data.ItemID.Equals(EnumItemCode.Blocked.ToString()))
            {
                //내가 수락한 퀘스트리스트의 퀘스트로 가서
                foreach (QuestContent_SO quest in questList)
                {
                    if (quest.Status == QuestStatus.Continue)//그 퀘스트의 상태가 진행중인지 확인한다. 진행중이면
                    {
                        //그 퀘스트의 목표 아이템과 비교한다.
                        if (quest.Contents.ItemCode.ToString().Equals(item.data.ItemID))//만약 같은 아이템이면
                        {
                            //지역변수생성해서 그 아이템의 개수 넣고
                            int itemQuantity = inventoryItem.Count;
                        
                            //플레이어가 모은 아이템 개수에 넣는다.
                            quest.PlayerCollectCount = itemQuantity;
                        
                            //퀘스트창 업데이트 준비
                            GameObject count = button_ByQuest[quest].transform.Find("Count/TXT").gameObject;
                            if (count.TryGetComponent(out TextMeshProUGUI countTxt))
                            {
                                //만약, 플레이어가 모은 아이템 개수가 퀘스트가 요구하는 개수보다 적으면
                                if (itemQuantity < quest.Contents.TargetGoalCount)
                                {
                                    countTxt.text = $"{quest.PlayerCollectCount} / {quest.Contents.TargetGoalCount}";
                                }
                                else
                                {
                                    countTxt.text = $"{quest.Contents.TargetGoalCount} / {quest.Contents.TargetGoalCount}";
                                }
                            }
                            //완료여부를 체크한다.
                            CompleteQuest(quest, itemQuantity);
                            if (quest.Status == QuestStatus.Complete)
                            {
                                return;
                            }
                        }
                        //다른아이템이면 다음 퀘스트를 확인한다.
                    }
                }
                
            }
            return;
        }*/
    }

    private void CompleteQuest(QuestContent_SO quest, int itemQuantity)
    {
        if (quest.Contents.TargetGoalCount == itemQuantity)
        {
            quest.QuestComplete();
        }
    }
}

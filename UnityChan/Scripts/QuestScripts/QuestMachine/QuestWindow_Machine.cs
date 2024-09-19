using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestWindow_Machine : MonoBehaviour
{
    public static QuestWindow_Machine Instance;

    [SerializeField] private GameObject questWindow;
    [SerializeField] private GameObject questMachineButton;
    [SerializeField] private Button questListButton;
    [SerializeField] private GameObject questDetailWindow;
    
    private QuestContent_SO[] DoingQuest { get; set; }
    
    private readonly int capacity = 10;

    private Dictionary<Button, QuestContent_SO> buttonByQuest;

    private Button[] questListButtons;

    private Button ShowingDetail;
    
    private bool CanActive { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DoingQuest = new QuestContent_SO[capacity];
            buttonByQuest = new Dictionary<Button, QuestContent_SO>();
            questListButtons = new Button[capacity];

        }
        else
        {
            Debug.Log("Critical Error: QuestWindow_Machine");
        }
    }

    // Start is called before the first frame update
    /*void Start()
    {
        
    }*/

    // Update is called once per frame
    void Update()
    {
        if (CanActive)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                // 켜져있으면 끈다.
                if (questWindow.activeInHierarchy)
                {
                    foreach (Transform child in questWindow.transform)
                    {
                        child.gameObject.SetActive(false);
                    }
                    questWindow.SetActive(!questWindow.activeSelf);
                    if (questDetailWindow.activeInHierarchy)
                    {
                        questDetailWindow.SetActive(false);    
                    }
                    questMachineButton.SetActive(true);
                    CharacterInput.Instance.Input_Block = false;
                    return;
                }
                questWindow.SetActive(!questWindow.activeSelf);
                CharacterInput.Instance.Input_Block = true;
                foreach (Transform child in questWindow.transform)
                {
                    child.gameObject.SetActive(true);
                }
                questMachineButton.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.CompareTag("Player"));
        if (other.CompareTag("Player"))
        {
            questMachineButton.SetActive(true);
            CanActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            questMachineButton.SetActive(false);
            if (questWindow.activeInHierarchy)
            {
                questWindow.SetActive(false);
                questDetailWindow.SetActive(false);
            }
            CharacterInput.Instance.Input_Block = false;
            CanActive = false;
        }
    }

    public void QuestListUpdate(QuestContent_SO questMy)
    {
        for (int i = 0; i < capacity; i++)
        {
            if (DoingQuest[i] == null)
            {
                DoingQuest[i] = questMy;
                //Debug.Log(DoingQuest[i].NPC.Name);//현우인경우
                questListButtons[i] = CreateQuestListButton(i); //현우버튼
                buttonByQuest[questListButtons[i]] = DoingQuest[i];//딕셔너리 0번버튼 생성 - 현우
                break;
            }
        }
        QuestMachineListUpdate();//퀘스트머신리스트
    }

    private void QuestMachineListUpdate()
    {
        /*Debug.Log("ListUpdateStart");*/
        //서식지정자 사용하기 for문 i 됐을 때, i.ToString("D3"); // 표기를 세자리까지 표기, ex)003
        for (int i = 0; i < questListButtons.Length; i++)
        {
            if (questListButtons[i] != null)
            {
                GameObject indexText = questListButtons[i].transform.Find("Quest_Index/Index").gameObject;
                if (indexText.TryGetComponent(out TextMeshProUGUI index))
                {
                    /*int i = Array.IndexOf(DoingQuest, questMy) + 1;*/
                    int questIndex = Array.IndexOf(DoingQuest, buttonByQuest[questListButtons[i]]) + 1;
                    index.text = questIndex.ToString("D3"); 
                    /*Debug.Log("ListIndexUpdate");*/
                }
                GameObject titleText = questListButtons[i].transform.Find("Quest_Title/Title").gameObject;
                if (titleText.TryGetComponent(out TextMeshProUGUI title))
                {
                    title.text = buttonByQuest[questListButtons[i]].Contents.Title;
                    /*Debug.Log("ListTitleUpdate");*/
                }
                GameObject npcText = questListButtons[i].transform.Find("Quest_NPC/NPC").gameObject;
                if (npcText.TryGetComponent(out TextMeshProUGUI npc))
                {
                    npc.text = buttonByQuest[questListButtons[i]].NPC.Name;
                    /*Debug.Log("ListNpcNameUpdate");*/
                }
                GameObject questStatus = questListButtons[i].transform.Find("Quest_IsComplete/IsComplete").gameObject;
                if (questStatus.TryGetComponent(out TextMeshProUGUI status))
                {
                    status.text = buttonByQuest[questListButtons[i]].Status.ToString();  
                    /*Debug.Log("ListIsCompleteUpdate");*/
                }
            }
        }
        
    }

    public void ShowQuestDetail(Button clickedButton)
    {
        if (questDetailWindow.activeInHierarchy)
        {
            if (ShowingDetail.Equals(clickedButton))//퀘스트머신리스트에서 같은 퀘스트를 눌렀을 때
            {
                questDetailWindow.SetActive(!questDetailWindow.activeInHierarchy);
                return;    
            }
            //다른버튼을 눌렀을 때
            QuestDetailUpdate(clickedButton);
        }
        else
        {
            QuestDetailUpdate(clickedButton);
            questDetailWindow.SetActive(!questDetailWindow.activeInHierarchy);
        }
        ShowingDetail = clickedButton;//눌렀던 버튼 저장
    }
    
    private void QuestDetailUpdate(Button clickedButton)
    {
        QuestContent_SO showingDetail = buttonByQuest[clickedButton];
        //퀘스트머신 자체가 수주가능한 퀘스트의 리스트를 들고있어야할듯?
        //그리고 그 리스트에서 뽑은것들만 명단에 올리는 식으로
        GameObject npcIcon = questDetailWindow.transform.Find("NpcIcon").gameObject;
        if (npcIcon.TryGetComponent(out Image icon))
        {
            /*Debug.Log(icon);*/
            //충격, 공포, 경악
            icon.sprite = showingDetail.NPC.SDCharacterIcon;
        }
        GameObject titleText = questDetailWindow.transform.Find("QuestDetail_Title").gameObject;
        if (titleText.TryGetComponent(out TextMeshProUGUI title))
        {
            title.text = $"퀘스트: {showingDetail.Contents.Title}";
        }
        GameObject targetText = questDetailWindow.transform.Find("QuestDetail_Target").gameObject;
        if (targetText.TryGetComponent(out TextMeshProUGUI target))
        {
            target.text = $"목표: {showingDetail.Contents.TargetGoalCount} 개";
        }
        GameObject rewardText = questDetailWindow.transform.Find("QuestDetail_Reward").gameObject;
        if (rewardText.TryGetComponent(out TextMeshProUGUI reward))
        {
            reward.text = $"보상: {showingDetail.Contents.Reward}";/*SelectedQuest.Reward.ToString();*/
        }
        GameObject contentText = questDetailWindow.transform.Find("QuestDetail_Content").gameObject;
        if (contentText.TryGetComponent(out TextMeshProUGUI content))
        {
            content.text = showingDetail.Contents.QuestDetail;
        }
    }

    private Button CreateQuestListButton(int index)
    {
        Button button = Instantiate(questListButton, questWindow.transform, false);
        button.name = $"QuestListButton{index}";
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(780, 40);
        rectTransform.anchoredPosition = new Vector2(0, -30 - 50*index);
        
        Button buttonComponent = button.GetComponent<Button>();
        buttonComponent.onClick.AddListener(() => Instance.ShowQuestDetail(button));
        return button;
    }

    public void Accept()
    {
        Debug.Log("퀘스트 수락");
        if (PlayerQuestWindow.Instance != null)
        {
            //buttonByQuest[ShowingDetail] 여기 까지는 Quest_my 이다. 내가 디테일을 보기위하 선택한 퀘스트
            buttonByQuest[ShowingDetail].QuestContinue();
            //최초에 퀘스트가 있던 리스트는 디클라인 퀘스트리스트.
            QuestStateBox.Instance.UpdateDeclineQuestList();
            Debug.Log(QuestStateBox.Instance.DeclineQuestList_SO.Count);
            QuestMachineListUpdate();
            PlayerQuestWindow.Instance.PlayerQuestListContinueUpdate(buttonByQuest[ShowingDetail]);
            PlayerQuestWindow.Instance.QuestStatusCheck();
            questDetailWindow.SetActive(!questDetailWindow.activeInHierarchy);
            Debug.Log(buttonByQuest[ShowingDetail].NPC.Name);
        }
    }

    public void Decline()
    {
        if (PlayerQuestWindow.Instance != null)
        {
            if (buttonByQuest[ShowingDetail].Status.Equals(QuestStatus.Complete))
            {
                //추가 알람 로직 있으면 좋을듯?
                questDetailWindow.SetActive(!questDetailWindow.activeInHierarchy);
                return;
            }
            if (buttonByQuest[ShowingDetail].Status.Equals(QuestStatus.Continue))
            {
                //진행중인 퀘스트를 다시 디클라인하면 
                buttonByQuest[ShowingDetail].QuestDecline();
                //컨티뉴 퀘스트리스트를 업데이트한다
                QuestStateBox.Instance.UpdateContinueQuestList();
                Debug.Log(QuestStateBox.Instance.ContinueQuestList_SO.Count);
                //퀘스트머신리스트를 업데이트한다.
                QuestMachineListUpdate();
                PlayerQuestWindow.Instance.PlayerQuestListDeclineUpdate(buttonByQuest[ShowingDetail]);
            }
            questDetailWindow.SetActive(!questDetailWindow.activeInHierarchy);
            Debug.Log("퀘스트 보류");
        }
    }
}

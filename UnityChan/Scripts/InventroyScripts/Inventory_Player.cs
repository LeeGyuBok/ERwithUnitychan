using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Inventory_Player : MonoBehaviour
{
    //오직 하나!
    private static Inventory_Player instance;

    public static Inventory_Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Inventory_Player>();
                
                if (instance == null)
                {
                    GameObject singletonObj = new GameObject("Inventory_Player");
                    instance = singletonObj.AddComponent<Inventory_Player>();
                }
                
                DontDestroyOnLoad(instance);
            }

            return instance;
        }
    }
    
    //인벤토리 패널
    [SerializeField] private GameObject InventoryPanel;
    //아이템 디테일 패널
    [SerializeField] private GameObject ItemDetailPanel;
    
    
    
    //Ui용 버튼
    private Button[] InventorySpace;
    
    //인벤토리 자체의 최대 용량 != 아이템 수량
    private int inventoryDataCapacity = 16;
    
    //Ui용 버튼과 실제 데이터를 연결함
    private Dictionary<Button, Stack<Item_SO>> Inventory;
    
    /*실제 데이터
    인벤토리내의 아이템 수량을 포함한 실제 인벤토리
    인벤토리내의 아이템과 그 수량을 나타내는 인벤토리 한 칸*/
    public Stack<Item_SO>[] InventoryData { get; private set; }
    public Stack<Item_SO> InventoryDataQuantity{ get; private set; }

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
        InventoryData = new Stack<Item_SO>[inventoryDataCapacity];
        //초기 셋팅 빈칸 8개, 막힌 칸 8개
        for (int i = 0; i < inventoryDataCapacity; i++)
        {
            //일단 인벤토리에 스택을 만든다.

            if (i < 8)
            {
                /*아이템 타입의 스택을 만들고
                 Blank를 넣어준 뒤
                 인벤토리에 넣어준다.*/
                InventoryDataQuantity = new Stack<Item_SO>(1);
                InventoryDataQuantity.Push(ItemManager_SO.Instance.GetItem(0));
                InventoryData[i] = InventoryDataQuantity;
            }
            else
            {
                /*아이템 타입의 스택을 만들고
                 Blocked를 넣어준 뒤
                 인벤토리에 넣어준다.*/
                InventoryDataQuantity = new Stack<Item_SO>(1);
                InventoryDataQuantity.Push(ItemManager_SO.Instance.GetItem(1));
                InventoryData[i] = InventoryDataQuantity;
            }
        }
        InventorySpace = InventoryPanel.GetComponentsInChildren<Button>();
        InventorySpace[0].onClick.AddListener(Switch);
        
        Inventory = new Dictionary<Button, Stack<Item_SO>>();
        
        for (int i = 0; i < inventoryDataCapacity; i++)
        {
            Button currentButton = UpdateInventory(i);
            currentButton.onClick.AddListener(() => InventorySpaceClick(currentButton));
            currentButton.gameObject.AddComponent<MouseHover>();//마우스 호버시 정보 출력 위한 스크립트
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (InventoryPanel)
            {
                if (!CharacterInput.Instance.Input_Block)
                {
                    CharacterInput.Instance.Input_Block = true;
                    Debug.Log("Input block");
                    InventoryPanel.SetActive(!InventoryPanel.activeSelf); // UI 창의 활성화 상태를 반전시킵니다.
                    return;
                }
                if (CharacterInput.Instance.Input_Block)
                {
                    CharacterInput.Instance.Input_Block = false;
                    Debug.Log("Input pass");
                    InventoryPanel.SetActive(!InventoryPanel.activeSelf); // UI 창의 활성화 상태를 반전시킵니다.
                }
            }
        }
        /*//템창 초기화용코드 나중에 삭제할것
        if (Input.GetKeyDown(KeyCode.J))
        {
            Start();
        }*/
    }
    /// <summary>
    /// Update Inventory. 
    /// </summary>
    /// <param name="inventorySpaceNumber"></param>
    /// <returns></returns>
    private Button UpdateInventory(int inventorySpaceNumber)
    {
        //Debug.Log(InventorySpace.Length);
        //여기서 에러가 발생했다면, InventoryPanel에 Inventory_Player오브젝트가 할당되었는지 확인하세요
        Button currentInventorySpace = InventorySpace[inventorySpaceNumber + 1];
        Stack<Item_SO> currentItemStack = InventoryData[inventorySpaceNumber];
        Inventory[currentInventorySpace] = currentItemStack;
        UpdateItemImage(currentInventorySpace);
        UpdateItemQuantity(currentInventorySpace);
        return currentInventorySpace;
    }
    /// <summary>
    /// Update itemImage in Inventory.
    /// </summary>
    /// <param name="currentInventorySpace"></param>
    private void UpdateItemImage(Button currentInventorySpace)
    {
        if (currentInventorySpace.TryGetComponent(out Image inventorySpaceImage))
        {
            inventorySpaceImage.sprite = Inventory[currentInventorySpace].Peek().Icon;    
        }
        /*Image inventorySpaceImage = currentInventorySpace.GetComponent<Image>();*/
    }
    /// <summary>
    /// Update DropQuantity in Inventory.
    /// </summary>
    /// <param name="currentInventorySpace"></param>
    private void UpdateItemQuantity(Button currentInventorySpace)
    {
        TextMeshProUGUI itemQuantity = currentInventorySpace.GetComponentInChildren<TextMeshProUGUI>();
        Item_SO item = Inventory[currentInventorySpace].Peek();
        //아이템이 빈칸 또는 막혀있음 이면 
        if (item.data.ItemID.Equals(0.ToString()) || item.data.ItemID.Equals(1.ToString()))
        {
            //수량 표시 X
            itemQuantity.text = "";
            return;
        }
        itemQuantity.text = $"{Inventory[currentInventorySpace].Count}";
        //수락한 퀘스트가 있는 경우에만 체크. List.Any() -> 리스트가 비어있는지 확인. 요소가 있으면 true 없으면 false
        if (!PlayerQuestWindow.Instance.questList.Equals(null))
        {
            PlayerQuestWindow.Instance.QuestStatusCheck();    
        }
        
    }

    public void GetItem(Item_SO item)
    {
        for (int i = 0; i < InventoryData.Length; i++)//인벤토리 내부순회
        {
            if (EnterInventory_Blank(item, i, out var currentItem)) return;//일단 넣으면 순환할 필요 없음.

            if (currentItem.data.Name.Equals(item.data.Name))//같은 아이템인 경우
            {
                //같은 아이템인데 Quantity가 MaxQuantity보다 작은 경우
                if (InventoryData[i].Count < item.data.MaxQuantity)
                {
                    for (int j = 0; j < item.data.DropQuantity; j++)
                    {
                        InventoryData[i].Push(item); //일단 넣어
                        UpdateItemQuantity(InventorySpace[i + 1]); //수량만 변화
                        if (InventoryData[i].Count > item.data.MaxQuantity) //100 > 99 가 되버린 경우. 그냥 아이템 버린다.
                        {
                            InventoryData[i].Pop();
                            UpdateItemQuantity(InventorySpace[i + 1]);
                        }
                    }
                }
                else//동일하거나 큰 경우
                {
                    continue;//인벤토리 재순회
                }
                return;
            }
            //만약 98개야. 100개가 되면 위 if문 조건절을 만족하지 않아서 바로 내려옴 그럼 다음칸은? 블랭크야. 그럼 거기에 2개들어감
            if (currentItem.data.Name.Equals(EnumItemCode.Blocked.ToString()))
            {
                Debug.Log("Blocked");
                item = null;
                return;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item">item, maybe enter to inventory</param>
    /// <param name="i">inventory space number</param>
    /// <param name="currentItem">item, already in inventory space number</param>
    /// <returns></returns>
    private bool EnterInventory_Blank(Item_SO item, int i, out Item_SO currentItem)
    {
        currentItem = InventoryData[i].Peek();
        if (currentItem.data.Name.Equals(EnumItemCode.Blank.ToString()))//비어있는 칸에 넣는 것
        {
            InventoryData[i].Clear();
            for (int j = 0; j < item.data.DropQuantity; j++)
            {
                InventoryData[i].Push(item);    
            }
            UpdateInventory(i);
            return true;
        }

        return false;
    }

    public void InventorySpaceClick(Button clickedButton)
    {
        Debug.Log(Inventory[clickedButton].Peek().data.Name + " clicked!");
    }
    
    public void Switch()
    {
        CharacterInput.Instance.Input_Block = false;  
        Debug.Log("Input pass");
        InventoryPanel.SetActive(!InventoryPanel.activeSelf); // UI 창의 활성화 상태를 반전시킵니다.
    }

    private void DestroyItem(Item_My item)
    {
        item = null;
    }

    public void ShowDetail(string buttonName)
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (InventorySpace[i+1].name.Equals(buttonName))
            {
                if (Inventory[InventorySpace[i+1]].Peek().data.Name.Equals(EnumItemCode.Blank.ToString())
                    || Inventory[InventorySpace[i+1]].Peek().data.Name.Equals(EnumItemCode.Blocked.ToString()))
                {
                    return;
                }
                Debug.Log($"{Inventory[InventorySpace[i+1]].Peek().data.Name} hovered!");
                Transform itemName = ItemDetailPanel.transform.Find("ItemName");
                Transform itemDetail = ItemDetailPanel.transform.Find("ItemDetail");
                TextMeshProUGUI itemKoreanName = itemName.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI itemKoreanDetail = itemDetail.GetComponent<TextMeshProUGUI>();
                itemKoreanName.text = $"{Inventory[InventorySpace[i+1]].Peek().data.KoreanName}";
                itemKoreanDetail.text = $"{Inventory[InventorySpace[i + 1]].Peek().data.KoreanName}";
                ItemDetailPanel.SetActive(!ItemDetailPanel.activeSelf);
                return;
            }
        }
    }
    
    public void HideDetail()
    {
        //하이어라키에서 켜져있으면
        if (ItemDetailPanel.activeInHierarchy)
        {
            ItemDetailPanel.SetActive(!ItemDetailPanel.activeSelf);    
        }
    }
}

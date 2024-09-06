using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Inventory_NoUse : MonoBehaviour
{
    //오직 하나!
    public static Inventory_NoUse Instance;

    //인벤토리 패널
    [SerializeField] private GameObject InventoryPanel;
    
    //최대 용량
    private int inventoryDataCapacity = 16;
    
    //실제 데이터 - 구버전
    private Item_My[] InventoryData;

    
    //Ui용 버튼
    private Button[] InventorySpace;
    
    //Ui용 버튼과 실제 데이터를 연결함
    private Dictionary<Button, Item_My> Inventory;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            InventoryData = new Item_My[inventoryDataCapacity];
            //초기 셋팅 빈칸 8개, 막힌 칸 8개
            for (int i = 0; i < inventoryDataCapacity; i++)
            {
                //일단 인벤토리에 스택을 만든다.
                
                if (i < 8)
                {
                    InventoryData[i] = ItemPool.Instance.DropItem(0);
                }
                else
                {
                    InventoryData[i] = ItemPool.Instance.DropItem(1);
                }
                
            }
            /*Ui용 버튼을 모두 연결 이때, Close버튼까지 17개 이므로 
            [0]은 CloseButton 따라서 1부터16까지 그냥 사용하면됨*/
            InventorySpace = InventoryPanel.GetComponentsInChildren<Button>();
            
            /*//디버그용
             foreach (var t in InventorySpace)
            {
                Debug.Log(t.name);
            }*/
            
            //ButtonUI와 ItemData를 연결해요 - 초기 아이템 셋팅자료를 딕셔너리와 연결해요
            Inventory = new Dictionary<Button, Item_My>();
            for (int i = 0; i < inventoryDataCapacity; i++)
            {
                /*//폐기한 초기 코드
                 Inventory[InventorySpace[i + 1]] = InventoryData[i];
                Button currentButton = InventorySpace[i + 1];
                SynchronizeItem(currentButton);*/
                /*싱크로나이즈아이템 - 딕셔너리<버튼, 아이템>에서 아이템 담당 배열에 할당된 i번째 아이템(값)을 i+1번째 버튼(키)에 연결(코드적)
                 예시) 아이템 배열의 3번에 A아이템이 있다면, 3번 인벤토리스페이스버튼에 A아이템을 연결
                 결과적으로 인벤토리창의 3번째 칸에 A아이템의 이미지가 노출*/
                Button currentButton = SynchronizeItem(i);
                currentButton.onClick.AddListener(() => InventorySpaceClick(currentButton));
                currentButton.gameObject.AddComponent<MouseHover>();//마우스 호버시 정보 출력 위한 스크립트
            }
            //Debug.Log("Initializing Complete");
        }
        else
        {
            Debug.Log("Critical Error: Inventory_NoUse");
        }
    }

    /*private void Start()
    {
        //껐다켜는 것은 start, Awake가 영향을 주지 않음. 최초 생성, 시작시에만 영향을 줌
        //Debug.Log("Start");
    }*/

    private void Update()
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
    }

    public void GetItem(Item_My item)
    {
        for (int i = 0; i < InventoryData.Length; i++)//인벤토리 내부순회 시작
        {
            if (InventoryData[i].Name.Equals(EnumItemCode.Blank.ToString()))//아이템이 없는경우
            {
                InventoryData[i] = item;
                SynchronizeItem(i);
                return;
            }
            
            if(InventoryData[i].Name.Equals(EnumItemCode.Blocked.ToString())) //인벤토리가 막혀있는 경우
            {
                //못넣는다는 알림. 더이상 넣을 수 없으니 순회필요없음. 해당 아이템 삭제.
                item = null;
                return;
            }
                
            if (InventoryData[i].Name.Equals(item.Name))//만약 획득한 아이템이 기존에 갖고있던 아이템과 같은 경우
            {
                if (InventoryData[i].Quantity >= InventoryData[i].MaxQuantity)
                {
                    continue;
                }
                UpdateItemQuantity2(InventoryData[i], i+1, item);
                
                
                //기본소지량간 합산 후 최대소지량보다 작으면 패스, 크면 최대소지량빼고 다음칸으로 남은 양 만큼 넘기기
            }
            //아이템이 있는경우 다음칸[i+1] 확인하기
        }
    }

    /*public Item_My ExtractItem()
    {
        //마우스로 클릭한 아이템을 지정한다
        Item_My item = InventoryData[0];
        return item;
    }*/

    //닫기 버튼.
    public void Switch()
    {
        CharacterInput.Instance.Input_Block = false;  
        Debug.Log("Input pass");
        InventoryPanel.SetActive(!InventoryPanel.activeSelf); // UI 창의 활성화 상태를 반전시킵니다.
    }

    //역할: 아이템의 자세한 정보 나타내기
    public void InventorySpaceClick(Button clickedButton)
    {
        Debug.Log(Inventory[clickedButton].Name + " clicked!");
    }

    /// <summary> 
    /// Input an image to button u want.원하는 버튼에 이미지를 입력하세요.
    /// </summary>
    /// <param name="button">Location of Inventory</param>
    public void SynchronizeItem(Button button)
    {
        Image buttonImage = button.GetComponent<Image>();
        buttonImage.sprite = Inventory[button].Icon;
    }
    
    /// <summary> 
    /// Update item data in itemArray to dictionary, ui.
    /// </summary>
    /// <param name="inventorySpaceNumber">Location of InventorySpace</param>
    /// <returns>button changed Image of Item in InventoryData[inventorySpaceNumber]</returns>
    public Button SynchronizeItem(int inventorySpaceNumber)
    {
        Button currentInventorySpace = InventorySpace[inventorySpaceNumber + 1];
        Item_My currentItem = InventoryData[inventorySpaceNumber];
        Inventory[currentInventorySpace] = currentItem;
        Image inventorySpaceImage = currentInventorySpace.GetComponent<Image>();
        inventorySpaceImage.sprite = Inventory[currentInventorySpace].Icon;
        if (inventorySpaceImage.sprite.name.Equals("Blank") || inventorySpaceImage.sprite.name.Equals("Blocked"))
        {
            UpdateItemQuantity(currentInventorySpace);    
        }
        else
        {
            UpdateItemQuantity(currentInventorySpace, InventoryData[inventorySpaceNumber].Quantity);
        }
        
        return currentInventorySpace;
    }

    public void UpdateItemQuantity(Button currentInventorySpace, int quantity)
    {
        TextMeshProUGUI buttonText = currentInventorySpace.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = $"{quantity}";
    }
    
    public void UpdateItemQuantity(Button currentInventorySpace)
    {
        TextMeshProUGUI buttonText = currentInventorySpace.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "";

    }

    public void UpdateItemQuantity2(Item_My myItem, int inventorySpaceNumber, Item_My item)
    {
        TextMeshProUGUI inventoryItemQuantity = InventorySpace[inventorySpaceNumber].GetComponentInChildren<TextMeshProUGUI>();
        int quantitySum = myItem.Quantity + item.Quantity;
        if (myItem.MaxQuantity >= quantitySum)
        {
            inventoryItemQuantity.text = $"{quantitySum}";
        }
        else
        {
            inventoryItemQuantity.text = $"{myItem.MaxQuantity}";
            
            int quantityRemain = quantitySum - item.MaxQuantity;
            
            Button nextInventorySpace = InventorySpace[inventorySpaceNumber + 1];
            Item_My currentItem = InventoryData[inventorySpaceNumber];
            currentItem.Quantity = quantityRemain;
            Inventory[nextInventorySpace] = currentItem;
            
            Image inventorySpaceImage = nextInventorySpace.GetComponent<Image>();
            inventorySpaceImage.sprite = Inventory[nextInventorySpace].Icon;
            UpdateItemQuantity(nextInventorySpace, Inventory[nextInventorySpace].Quantity);
        }
    }
    
    /*초기 싱크로나이즈아이템 코드 - 폐기
     public void SynchronizeItem(int InventorySpaceNumber, int itemCode)
    {
        //가지고 있는 아이템 데이터의 배열에 아이템을 넣는다.
        /*Item_My[]#1#InventoryData[InventorySpaceNumber-1] = ItemPool.Instance.DropItem(itemCode);
        Item_My item = InventoryData[InventorySpaceNumber - 1];
        Button targetButton = InventorySpace[InventorySpaceNumber];
        
        //인벤토리 딕셔너리의 버튼에 해당 아이템을 다시 할당한다.
        /*Dictionary<Button, Item_My> #1#Inventory[targetButton] = item;
        Image buttonImage = targetButton.GetComponent<Image>();
        buttonImage.sprite = item.Icon;
    }*/
}

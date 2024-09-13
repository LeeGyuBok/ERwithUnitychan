using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnumItemCode
{
    //열거형 본격적으로 처음쓰니까 어떻게 쓰는지 알아보자
    /* 열거형값을 정수로
     * Enum_ItemCode myItem = Enum_ItemCode.Alcohol;
     * 열거타입 변수명 = 열거타입.열거값
     * int itemValue = (int)myItem;
     * 정수타입 변수명 = (정수타입 형변환)열거타입의 변수명
     *
     * 정수형값을 열거형값으로
     * int itemValue = 4;
     * 정수타입 변수명
     * Enum_ItemCode myItem = (Enum_ItemCode)itemValue;
     * 열거타입 변수명 = (열거타입 형변환)정수타입의 변수명
     *
     * 이 Enum을 기준으로, Blank는 0, Blocked는 1이다.
     */ 
    Blank,
    Blocked,
    Alcohol,
    Feather,
    Flower,
    Gold,
    Leather,
    Stone,
    VF_Blood_Sample//8, 총 9개
}

public class ItemPool_SO : MonoBehaviour
{
    [SerializeField] private List<ItemData_SO> itemDatabase;

    private ItemData_SO dropItemData;
    
    public List<ItemData_SO> ItemDataBase { get; }
    
    
     private void Awake()
    {   
        /*//디버그용    
        foreach (ItemData_SO data in itemDatabase)
        {
            Item_SO item = new Item_SO(data);
            item.ShowInfo();
        }*/
    }

    //data.ItemID를 인수로 받아 아이템을 드랍한다.
    //item을 내준다.
    public Item_SO DropItem(int itemID)
    {
        foreach (ItemData_SO data in itemDatabase)
        {
            if (data.ItemID.Equals(itemID.ToString()))
            {
                dropItemData = data;
                break;
            }
        }
        Item_SO item = new Item_SO(dropItemData);
        dropItemData = null;
        return item;
    }
}

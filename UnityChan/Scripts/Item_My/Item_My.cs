using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

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

public class Item_My
{
    public Sprite Icon;
    public string Name { get; private set; }
    public string KoreanName { get; private set; }
    public string KoreanDetail { get; private set; }
    public string ItemId { get; private set; }
    public int MaxQuantity { get; private set; }
    public int BasicQuantity { get; private set; }
    public int Quantity { get; set; }

    /// <summary>
    /// item, quantity is only 1. MaxQ == Quantity == 1.
    /// </summary>
    /// <param name="name">itemname must same iconname</param>
    /// <param name="koreanName"></param>
    /// <param name="koreanDetail"></param>
    /// <param name="itemId">serial number of item</param>
    public Item_My(string name, string koreanName, string koreanDetail, int itemId)
    {
        Name = name;
        KoreanName = koreanName;
        KoreanDetail = koreanDetail;
        //이미지의 텍스쳐타입을 Sprite로 바꿔야한다! Load<Sprite> 잖아! 우리는 타입의 노예다.
        Icon = Resources.Load<Sprite>($"Icons/{name}");
        ItemId = itemId.ToString();
        MaxQuantity = 1;
        BasicQuantity = MaxQuantity;
        Quantity = BasicQuantity;
        
        Debug.Log(Icon);
    }

    /// <summary>
    /// item, basicQuantity can over 1. MaxQ >= Quantity
    /// </summary>
    /// <param name="name">itemname must same iconname</param>
    /// <param name="koreanName"></param>
    /// <param name="koreanDetail"></param>
    /// <param name="itemId">serial number of item</param>
    /// <param name="maxQuantity">maximum 99 plz</param>
    /// <param name="basicQuantity">minimum 1, but over is ok</param>
    public Item_My(string name, string koreanName, string koreanDetail, int itemId, int maxQuantity, int basicQuantity)
    {
        Name = name;
        KoreanName = koreanName;
        KoreanDetail = koreanDetail;
        //이미지의 텍스쳐타입을 Sprite로 바꿔야한다! Load<Sprite> 잖아! 우리는 타입의 노예다.
        Icon = Resources.Load<Sprite>($"Icons/{name}");
        ItemId = itemId.ToString();
        MaxQuantity = maxQuantity;
        BasicQuantity = basicQuantity;
        Quantity = BasicQuantity; 
        Debug.Log(Icon);
    }

    
}

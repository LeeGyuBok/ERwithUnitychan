using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

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

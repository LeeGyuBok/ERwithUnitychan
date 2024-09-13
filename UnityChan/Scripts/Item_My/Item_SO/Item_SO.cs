using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_SO
{
    public Sprite Icon;
    
    //영어이름, 한국어이름, 아이템아이디, 아이템 최대수량, 1회 드랍량
    public ItemData_SO data;

    public Item_SO(ItemData_SO data_SO)
    {
        data = data_SO;
        Icon = Resources.Load<Sprite>($"Icons/{data_SO.Name}");
    }

    public void ShowInfo()
    {
        Debug.Log(data.ItemID);
        Debug.Log(data.Name);
        Debug.Log(data.KoreanName);
        Debug.Log(data.MaxQuantity);
        Debug.Log(data.DropQuantity);
        Debug.Log(Icon.name);
        Debug.Log("cccc");
    }
    
}

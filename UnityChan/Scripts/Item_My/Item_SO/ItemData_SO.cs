using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Item", fileName = "ItemData")]
public class ItemData_SO : ScriptableObject
{
    [SerializeField] private string name;
    public string Name
    {
        get { return name; }
    }

    [SerializeField] private string koreanName;
    public string KoreanName
    {
        get { return koreanName; }
    }

    [SerializeField] private string itemID;
    public string ItemID
    {
        get { return itemID; }
    }

    [SerializeField] private int maxQuantity;
    public int MaxQuantity
    {
        get { return maxQuantity; }
    }

    [SerializeField] private int dropQuantity;
    public int DropQuantity
    {
        get { return dropQuantity; }
    }
}

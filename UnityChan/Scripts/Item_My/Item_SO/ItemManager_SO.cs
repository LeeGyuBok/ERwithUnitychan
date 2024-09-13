using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ItemManager_SO : MonoBehaviour
{
    private static ItemManager_SO instance;
    public static ItemManager_SO Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ItemManager_SO>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("ItemManager_SO");
                    instance = singletonObject.AddComponent<ItemManager_SO>();
                }
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }

    public ItemPool_SO Items { get; private set;}

    private void Awake()
    {
        Items = FindObjectOfType<ItemPool_SO>();
    }

    public Item_SO GetItem(int itemID)
    {
        return Items.DropItem(itemID);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcPool : MonoBehaviour
{
    public static NpcPool Instance;
    public Npc[] npcArray { get; private set; }

    private int capacity;
    
    private Npc SelectedNpc { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Sprite[] sprites = Resources.LoadAll<Sprite>("NPC/OriginPlayable");
            capacity = sprites.Length;
            /*Debug.Log(capacity);//75*/
            npcArray = new Npc[capacity];
            for (int i = 0; i < capacity; i++)
            {
                npcArray[i] = new Npc(sprites[i].name, 0);
                Debug.Log(npcArray[i].Name);
                //75명의 Npc데이터 생성, 한국어 이름 x
            }
            /*Debug.Log("complete");*/
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/

    public Npc ShowNpc(string name)
    {
        for (int i = 0; i < capacity; i++)
        {
            if (npcArray[i].Name.Equals(name))
            {
                SelectedNpc = npcArray[i]; 
                break;
            }
        }
        return SelectedNpc;
    }
}

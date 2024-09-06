using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRenerManager : MonoBehaviour
{
    public static MapRenerManager Instance { get; private set; }
    private GameObject[] MapArray;
    private Transform playerLocation;
    private GameObject currentLocation;
    private GameObject lastLocation;
     
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Critical Error: MapRenderManager");
        }
        Transform mapTransform = GameObject.Find("Map").transform;
        MapArray = new GameObject[mapTransform.childCount];

        for (int i = 0; i < MapArray.Length; i++)
        {
            MapArray[i] = mapTransform.GetChild(i).gameObject;
            Debug.Log(MapArray[i].name);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

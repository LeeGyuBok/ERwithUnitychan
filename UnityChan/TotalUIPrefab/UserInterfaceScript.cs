using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceScript : MonoBehaviour
{
    private static UserInterfaceScript instance;

    public static UserInterfaceScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UserInterfaceScript>();
                if (instance == null)
                {
                    GameObject singletonObj = new GameObject("UserInterfaceScript");
                    instance = singletonObj.AddComponent<UserInterfaceScript>();
                }
                
                DontDestroyOnLoad(instance);
            }

            return instance;
        }
        
    }
    
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
    /*// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}

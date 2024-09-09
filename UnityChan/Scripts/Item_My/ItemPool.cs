using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// Alcohol is 2, feater 3, flower4, gold5, leather6, stone7, vfsample8
/// </summary>
public class ItemPool : MonoBehaviour
{
    private static ItemPool instance;
    
    public static ItemPool Instance
    { 
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ItemPool>();
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }

    public Item_My[] itemArray { get; private set; }
    public Dictionary<Item_My, string> ItemNameEngToKor { get; set; }

    private int capacity = 50;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            
            itemArray = new Item_My[capacity];
            ItemArrayUpdate();
            ItemNameEngToKor = new Dictionary<Item_My, string>();
            
            DontDestroyOnLoad(gameObject);
        }
    }
    
    /*private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
                    
            itemArray = new Item_My[capacity];
            ItemArrayUpdate();
            ItemNameEngToKor = new Dictionary<Item_My, string>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            for (int i = 0; i < capacity; i++)
            {
                Debug.Log(itemArray[i].KoreanName);
            }
        }

    }*/

    /*
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/

    /*XML주석 예시
    /// <summary> 
    /// 두 개의 정수를 더하여 반환합니다.
    /// </summary>
    /// <param name="a">더할 첫 번째 정수</param>
    /// <param name="b">더할 두 번째 정수</param>
    /// <returns>두 정수의 합을 반환합니다.</returns>*/
    
    /// <summary>
    /// return item what u want
    /// </summary>
    /// <param name="itemIndex"> serial number of item</param>
    /// <returns>item, class is Item_my</returns>
    /// <remarks>when the player get item, use this.
    /// </remarks>
    public Item_My DropItem(int itemIndex)
    {
        /*
         * 초기에 아이템 풀이 아이템을 생성(new) 하고 그 아이템을 할당하는 방식.
         */

        Item_My item = itemArray[itemIndex];
        /*Debug.Log($"Drop: {item}");*/
        return item;
    }
    
    private void ItemArrayUpdate()
    {
        itemArray[0] = new Item_My("Blank", "비어있음","비어있음", 0); 
        itemArray[1] = new Item_My("Blocked","닫혀있음", "닫혀있음", 1);
        itemArray[2] = new Item_My("Alcohol","알코올", "알코올이다. 술을 좋아하는 사람은 이게 정말 필요하겠는걸?",2, 99, 1);
        itemArray[3] = new Item_My("Feather","깃털", "부드러운 깃털이다. 어딘가에 사용할 수 있을지도 모른다.",3, 99, 1);
        itemArray[4] = new Item_My("Flower","꽃", "향기로운 꽃이다. 향이 진해서 이런걸 들고 다니면 위험할 것같은 느낌이... ", 4, 99, 2);
        itemArray[5] = new Item_My("Gold","금", "모래에 숨어있던 금이다. 이 귀한걸 누가 버렸을까? 아니, 어쩌면 이곳에서는 금이 귀하지 않을지도...", 5, 99, 2);
        itemArray[6] = new Item_My("Leather","가죽", "아직 가공이 되지 않은 거친 가죽이다. 어딘가에 사용할 수 있을지도 모른다.", 6, 99, 1);
        itemArray[7] = new Item_My("Stone","돌멩이", "길가에 굴러다니는 돌멩이다. 이런 흔해 빠진 걸 어디다 쓸 수 있을까?", 7, 99, 2);
        itemArray[8] = new Item_My("VF Blood Sample","VF 혈액 샘플", "실험용 혈액이다. 물론 실험체에게서도 얻을 수 있는데... 이게 도대체 왜..?", 8);
    }
}

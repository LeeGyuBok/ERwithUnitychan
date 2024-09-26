using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    [SerializeField] private GameObject interactionButton;
    public int ItemCode { private get; set; }
    
    private bool PlayerContact { get; set; }

    private readonly float coolTime = 3f;

    private bool onCoolTime;
    private bool CanAction { get; set; } = true;
    
    private GameObject ContactObject { get; set; }

    // Start is called before the first frame update

    private void Awake()
    {
        //Debug.Log(GameObject.Find("UserInterface").transform.GetChild(2).name);
        //유저인터페이스 게임오브젝트의 자식 오브젝트 3개중 가장 마지막이 인터액션버튼
        interactionButton = GameObject.Find("UserInterface").transform.GetChild(2).gameObject;
    }

    private void Update()
    {
        if (PlayerContact & !onCoolTime)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (CanAction)
                {
                    Item_SO dropItem = ItemManager_SO.Instance.GetItem(ItemCode);
                    //Debug.Log("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR");
                    /*Inventory_NoUse.instance.GetItem(dropItem);*/
                    Inventory_Player.Instance.GetItem(dropItem);
                    StartCoroutine(Cooldown());    
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            ContactObject = other.gameObject;
            PlayerContact = true;
            /*Debug.Log("contact");*/
            if (CanAction)
            {
                if (gameObject.TryGetComponent(out IPossibleInteraction  possibleInteraction))
                {
                    ItemCode = possibleInteraction.ItemCode;
                    interactionButton.SetActive(PlayerContact);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        /*if (OnCoolTime)
        {
            return;
        }*/
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerContact = false;
            interactionButton.SetActive(PlayerContact);
            ContactObject = null;
        }
    }
    
       private IEnumerator Cooldown()
        {
            //Debug.Log("Cooldown");
            //코루틴은 일드리턴 구문까지 실행하다가 일드리턴 구문에서 지정된 시간동안 멈추고 다시 진행한다.
            //현재는 오브젝트마다 쿨타임이 다르게 흐른다.
            onCoolTime = true;//자 쿨타임 돌아요
            CanAction = false;//쿨타임동안 나를 상호작용할 수 없어요
            interactionButton.SetActive(CanAction);//네, 버튼끌게요
            yield return new WaitForSeconds(coolTime);//쿨타임 시작해요 -> cooltime 동안 아래 구문이 실행되지 않다가
            onCoolTime = false;//아 쿨타임 다 됐어요
            CanAction = true;//쿨타임 끝났으니 나 상호작용할 수 있어요
            
            //혹시 플레이어가 닿아있나요? ->contact
            if (PlayerContact)//네 닿아있네요
            {
                interactionButton.SetActive(CanAction);//버튼 다시 켜주세요
            }
            //안 닿아있어요. 지나갈게요
            
        }
}

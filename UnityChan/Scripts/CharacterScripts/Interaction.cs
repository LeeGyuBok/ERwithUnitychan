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
    private bool CanAction { get; set; } = true;
    
    private GameObject ContactObject { get; set; }

    // Start is called before the first frame update

    private void Awake()
    {
        //Debug.Log(GameObject.Find("UserInterface").transform.GetChild(2).name);
        interactionButton = GameObject.Find("UserInterface").transform.GetChild(2).gameObject;
    }

    private void Update()
    {
        if (PlayerContact)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (CanAction)
                {
                    Item_My dropItem = ItemPool.Instance.DropItem(ItemCode);
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
            /*Debug.Log("contact");*/
            if (CanAction)
            {
                if (gameObject.TryGetComponent(out IPossibleInteraction  possibleInteraction))
                {
                    ItemCode = possibleInteraction.ItemCode;
                    PlayerContact = true;
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
            CanAction = false;
            interactionButton.SetActive(CanAction);
            yield return new WaitForSeconds(coolTime);
            CanAction = true;
            if (PlayerContact)
            {
                interactionButton.SetActive(CanAction);    
            }
            else
            {
                interactionButton.SetActive(false);
            }
            
        }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//이 스크립트가 붙은 오브젝트에 마우스를 올리면 아래에 정의된 함수가 실행됨
public class MouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsHovered { get; private set; } = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Inventory_Player.Instance.ShowDetail(gameObject.name);
        //아이템의 간략한 정보를 나타내기
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Inventory_Player.Instance.HideDetail();
    }
}

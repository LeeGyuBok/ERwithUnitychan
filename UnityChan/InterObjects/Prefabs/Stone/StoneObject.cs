using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneObject :MonoBehaviour, IPossibleInteraction
{
    public int ItemCode { get; } = (int)EnumItemCode.Stone;
    public string Detail { get; } = "길가에 굴러다니는 돌멩이다. 이런걸 어디다 쓸 수 있을까?";
}

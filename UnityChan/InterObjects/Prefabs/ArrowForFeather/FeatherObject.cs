using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherObject : MonoBehaviour, IPossibleInteraction
{
    public int ItemCode { get; } = (int)EnumItemCode.Feather;
    public string Detail { get; } = "부드러운 깃털이다. 어딘가에 사용할 수 있을지도 모른다.";
}
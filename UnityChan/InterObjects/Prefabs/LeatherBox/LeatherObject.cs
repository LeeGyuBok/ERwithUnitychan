using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeatherObject : MonoBehaviour, IPossibleInteraction
{
    public int ItemCode { get; } = (int)EnumItemCode.Leather;
    public string Detail { get; } = "아직 가공이 되지 않은 거친 가죽이다. 어딘가에 사용할 수 있을지도 모른다.";
}

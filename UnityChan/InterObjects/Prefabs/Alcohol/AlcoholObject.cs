using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlcoholObject : MonoBehaviour, IPossibleInteraction
{
    public int ItemCode { get; } = (int)EnumItemCode.Alcohol;
    public string Detail { get; } = "알코올이다. 술을 좋아하는 사람은 이걸 필요로 할지도?";
}

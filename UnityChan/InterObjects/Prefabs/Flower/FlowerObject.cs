using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerObject : MonoBehaviour, IPossibleInteraction
{
    public int ItemCode { get; } = (int)EnumItemCode.Flower;
    public string Detail { get; } = "향기로운 꽃이다. 글쎄, 향이 진해서 이런걸 들고 다니면 위험할 것같은 느낌이... ";
}

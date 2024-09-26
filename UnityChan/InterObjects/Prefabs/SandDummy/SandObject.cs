using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandObject : MonoBehaviour, IPossibleInteraction
{
    public int ItemCode { get; } = (int)EnumItemCode.Gold;
    public string Detail { get; } = "모래에 숨어있던 금이다. 이 귀한걸 누가 버렸을까? 아니, 어쩌면 이곳에서는 금이 귀하지 않은건가?";
}

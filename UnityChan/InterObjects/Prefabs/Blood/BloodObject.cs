using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodObject : MonoBehaviour, IPossibleInteraction
{
    public int ItemCode { get; } = (int)EnumItemCode.VF_Blood_Sample;
    public string Detail { get; } = "실험용 혈액이다. 물론 실험체에게서도 얻을 수 있는데... 이게 도대체 왜..?";
}

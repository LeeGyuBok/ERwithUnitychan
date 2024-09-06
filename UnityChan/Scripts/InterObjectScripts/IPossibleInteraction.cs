using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPossibleInteraction
{
    public int ItemCode { get; }
    public string Detail { get; }
}

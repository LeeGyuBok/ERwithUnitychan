using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log(gameObject.name);
            SceneSwitcher.Instance.SwitchToScene();
        }
    }
}

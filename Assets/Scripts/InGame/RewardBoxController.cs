using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class RewardBoxController : MonoBehaviour
{
    int PlayerLayer;
    const string stagPlayer = "Player";
    
    void Start()
    {
        PlayerLayer = LayerMask.NameToLayer(stagPlayer);
    }
    private void OnCollisionEnter(Collision other)
    {
           if (other.gameObject.layer.Equals(PlayerLayer))
        {

            EventManager.Instance.RewardBoxTrigger();
        }
    }
  

}

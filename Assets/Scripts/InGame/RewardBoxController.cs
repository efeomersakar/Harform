using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using DG.Tweening;

public class RewardBoxController : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 0.5f;
    [SerializeField] private float jumpDuration = 0.2f;
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
            Vector3 targetPosition = transform.position + Vector3.up * jumpHeight; 
            transform.DOMoveY(targetPosition.y, jumpDuration) 
                .SetEase(Ease.OutQuad) 
                .OnComplete(() =>  
                {
                    transform.DOMoveY(transform.position.y - jumpHeight, jumpDuration)  
                        .SetEase(Ease.OutCubic); 
                });

            EventManager.Instance.RewardBoxTrigger();
        }
    }


}

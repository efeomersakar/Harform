using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    public float moveDistance = 5f; 
    public float moveDuration = 2f;  
    private void Start()
    {
        MoveAI();
    }

    private void MoveAI()
    {
        transform.DOMoveX(transform.position.x + moveDistance, moveDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EventManager.Instance.EnemyAttacked();
            Debug.Log("Enemy, Player'a çarptı!");            
        }
    }
}

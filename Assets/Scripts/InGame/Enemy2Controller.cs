using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy2Controller : MonoBehaviour
{
    public float moveDistance = 2f;
    public float moveDuration = 1f;

    public float speed = 0.5f;
    private void Start()
    {
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        float startY = transform.position.y;

        transform.DOMoveY(startY + moveDistance, moveDuration)
            .SetLoops(-1, LoopType.Yoyo) 
            .SetEase(Ease.InOutSine); 
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EventManager.Instance.EnemyAttacked();
        }
    }
}

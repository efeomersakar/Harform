using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    public float moveDistance = 5f;
    public float moveDuration = 1f;
    public float speed = 0.5f;
    private void Start()
    {
        MoveEnemy();
    }

    private void MoveEnemy()
    {

        transform.DOMoveX(transform.position.x + moveDistance, moveDuration * speed)
                   .SetEase(Ease.InOutSine)
                   .SetLoops(-1, LoopType.Yoyo);

    }
    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (EventManager.Instance.currentState == EventManager.GameState.GameContinue)
            {
                EventManager.Instance.EnemyAttacked();
            }
           ;
        }
    }
}

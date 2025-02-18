using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossController : MonoBehaviour
{
    private float moveDistance = 9f;
    private float moveDuration = 5f;
    public float speed = 15f;

    private void Start()
    {
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        float startY = transform.position.y;
        float startX = transform.position.x;

        transform.DOMoveY(startY + moveDistance, moveDuration / speed)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                transform.DOMoveX(startX + moveDistance, moveDuration / speed)
                    .SetLoops(2, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        MoveEnemy();
                    });
            });
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EventManager.Instance.SetPlayerState(EventManager.PlayerState.PlayerGotKilled);
            DOVirtual.DelayedCall(1.2f, () =>
   {
       EventManager.Instance.SetState(EventManager.GameState.LevelFailed);
   });

        }
    }
}
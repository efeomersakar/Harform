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

        Sequence moveSequence = DOTween.Sequence();
        moveSequence.Append(transform.DOMoveY(startY + moveDistance, moveDuration / speed)
                            .SetEase(Ease.InOutSine))
                    .Append(transform.DOMoveY(startY, moveDuration / speed)
                            .SetEase(Ease.InOutSine)) // Y ekseni hareketini tamamlıyo
                    .Append(transform.DOMoveX(startX + moveDistance, moveDuration / speed)
                            .SetEase(Ease.InOutSine))
                    .Append(transform.DOMoveX(startX, moveDuration / speed)
                            .SetEase(Ease.InOutSine)) // X ekseni hareketini tamamlıyo
                    .SetLoops(-1); // Sonsuz döngü
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
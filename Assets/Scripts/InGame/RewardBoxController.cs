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

    private int PlayerLayer;
    const string stagPlayer = "Player";
    private bool isRewardGiven = false;


    void Start()
    {
        PlayerLayer = LayerMask.NameToLayer(stagPlayer);
    }
    private void PlayJumpAnimation()
    {
        Sequence boxJumpSequence = DOTween.Sequence();
        boxJumpSequence.Append(transform.DOMoveY(transform.position.y + jumpHeight, jumpDuration)
                                    .SetEase(Ease.OutQuad))
                        .Append(transform.DOMoveY(transform.position.y, jumpDuration)
                                    .SetEase(Ease.OutCubic));
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other == null || other.gameObject == null || transform == null) return;

        if (other.gameObject.layer.Equals(PlayerLayer) && !isRewardGiven)
        {
            if (other.contacts.Length > 0)
            {
                PlayJumpAnimation();
                if (this != null)
                {
                    GiveorDestroy();
                }
            }
        }
        else
        {

            isRewardGiven = true;
            if (transform != null)
            {
                PlayJumpAnimation();
            }
        }
    }

    private void GiveorDestroy()
    {
        int randomValue = UnityEngine.Random.Range(0, 100);

        if (EventManager.Instance != null && transform != null)
        {
            if (randomValue < 70)
            {
                EventManager.Instance.RewardBoxTrigger(transform.position);
            }
            else
            {
                DestroyBox();
            }
        }

    }
    private void DestroyBox()
    {

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Sequence scaleAndColorSequence = DOTween.Sequence();

            scaleAndColorSequence.Append(renderer.material.DOColor(Color.red, 0.5f))
                                   .Append(transform.DOScale(Vector3.zero, 0.5f)
                                                   .SetEase(Ease.InBack))
                                   .OnComplete(() =>
                                   {
                                       ObjectPool.Instance.ReturnRewardBoxToPool(this.gameObject);
                                   });
        }

    }
}


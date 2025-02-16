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

    public void OnCollisionEnter(Collision other)
    {
        if (other == null || other.gameObject == null || transform == null) return; // Null kontrolü

        if (other.gameObject.layer.Equals(PlayerLayer) && !isRewardGiven)
        {
            if (other.contacts.Length > 0) 
            {
                Vector3 collisionNormal = other.contacts[0].normal;
                if (collisionNormal.y > 0.5f)
                {
                    isRewardGiven = true;
                    if (transform != null)
                    {
                        Vector3 targetPosition = transform.position + Vector3.up * jumpHeight;
                        transform.DOMoveY(targetPosition.y, jumpDuration)
                            .SetEase(Ease.OutQuad)
                            .OnComplete(() =>
                            {
                                if (transform != null)
                                {
                                    transform.DOMoveY(transform.position.y - jumpHeight, jumpDuration)
                                        .SetEase(Ease.OutCubic);
                                }
                            });

                        if (this != null) // Eğer obje hâlâ sahnedeyse
                        {
                            GiveorDestroy();
                        }
                    }
                }
            }
        }
        else
        {
            isRewardGiven = true;
            if (transform != null)
            {
                Vector3 targetPosition = transform.position + Vector3.up * jumpHeight;
                transform.DOMoveY(targetPosition.y, jumpDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        if (transform != null)
                        {
                            transform.DOMoveY(transform.position.y - jumpHeight, jumpDuration)
                                .SetEase(Ease.OutCubic);
                        }
                    });
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

            renderer.material.DOColor(Color.red, 0.5f);
        }

        transform.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }
}



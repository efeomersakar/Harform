using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WallScript : MonoBehaviour
{
    public float moveDistance = 5f;
    public float duration = 1f;
    public float speed = 5;
    private Vector3 startPosition;

    void OnEnable()
    {
        EventManager.Instance.OnPlayerGotDamaged += WallStartPosition;

    }
    void OnDisable()
    {
        EventManager.Instance.OnPlayerGotDamaged -= WallStartPosition;

    }

    void Start()
    {

        startPosition = transform.position;
        transform.DOMoveX(transform.position.x + moveDistance, duration * speed);

    }

    private void WallStartPosition()
    {
        transform.position = startPosition;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EventManager.Instance.EnemyAttacked();
        }
    }
}

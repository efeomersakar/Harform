using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Numerics;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody rb;
    private UnityEngine.Vector3 startPosition;



    void OnEnable()
    {
        EventManager.Instance.OnPlayerGotDamaged += PlayerStartPosition;
        EventManager.Instance.OnPlayerKilled += PlayDeathAnimation;

    }

    void OnDisable()
    {
        EventManager.Instance.OnPlayerGotDamaged -= PlayerStartPosition;
        EventManager.Instance.OnPlayerKilled -= PlayDeathAnimation;
    }
    //=====================================================================

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }
    //=====================================================================
    void Update()
    {
        if (EventManager.Instance.currentState == EventManager.GameState.GameContinue)
        {
            HandleMovement();
        }
    }

    //==================================================================================

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        UnityEngine.Vector3 movement = new UnityEngine.Vector3(moveX, moveY, 0);
        transform.Translate(movement * moveSpeed * Time.deltaTime);

    }
    //=========================================================
    private void PlayerStartPosition()
    {
        if (GameManager.Instance.lives >= 1)
        {
            transform.position = startPosition;
        }
    }
    private void PlayDeathAnimation()
    {
        Sequence deathAnimation = DOTween.Sequence();
        deathAnimation.Append(transform.DOScale(2f, 0.9f).OnStart(() => 
            GetComponent<Renderer>().material.DOColor(Color.red, 0.1f)));
        deathAnimation.Append(transform.DOScale(0f, 0.1f));
        deathAnimation.AppendCallback(() => Destroy(gameObject));

    //     transform.DOScale(2f, 0.9f)
    //   .OnStart(() =>
    //   {
    //       GetComponent<Renderer>().material.DOColor(Color.red, 0.1f);
    //   })
    //   .OnComplete(() =>
    //   {
    //       transform.DOScale(0f, 0.1f)
    //           .OnComplete(() =>
    //           {
    //               Destroy(gameObject);
    //           });
    //   });
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // DOTween kütüphanesi eklenmeli

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] private float jumpForce = 30f;
    private Rigidbody rb;
    private PlayerState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation; //2D perpektif için Z rotasyonunu dondurduk
        currentState = PlayerState.Idle;
    }
    //=====================================================================
    void Update()
    {
        HandleState();
        HandleMovement();
    }

    //====================================================================
    void HandleState()
    {
        PlayerState newState = currentState;

        switch (currentState)
        {
            case PlayerState.Idle:
                if (IsJumping())
                {
                    newState = PlayerState.Jumping;
                }
                else if (IsWalking())
                {
                    newState = PlayerState.Walking;
                }

                break;

            case PlayerState.Walking:
                if (IsJumping())
                {
                    newState = PlayerState.Jumping;
                }
                else if (!IsWalking())
                {
                    newState = PlayerState.Idle;
                }
                break;

            case PlayerState.Jumping:
                if (IsGrounded())
                {
                    newState = PlayerState.Idle;
                }
                break;
        }

        if (newState != currentState)
        {
            ChangeState(newState);
        }
    }
    //=====================================================================

    private void ChangeState(PlayerState newState)
    {
        currentState = newState;
        EventManager.Instance?.PlayerStateChangeEvent(newState);

        Debug.Log("Yeni State: " + newState);
    }

    //==================================================================================

    private bool IsWalking()
    {
        return Input.GetAxis("Horizontal") != 0;
    }
    //==================================================================================
    private bool IsJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            transform.DOScale(new Vector3(1.1f, 0.9f, 1f), 0.1f)  
                .OnComplete(() =>
                {
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                });

            return true;
        }
        return false;
    }


    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }


    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        Vector3 targetVelocity = new Vector3(moveX * moveSpeed, rb.velocity.y, 0);
        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, 0.2f);

        // if (moveX != 0)
        // {
        //     transform.DORotate(new Vector3(0, 0, moveX * -10f), 0.2f)
        //         .SetEase(Ease.OutQuad);

        //     transform.DOScale(new Vector3(1.1f, 0.9f, 1f), 0.2f)
        //         .SetEase(Ease.OutQuad);
        // }
        // else
        // {
        //     transform.DORotate(Vector3.zero, 0.2f);
        //     transform.DOScale(Vector3.one, 0.2f);
        // }
    }


    //================================================================
    //=========================================================
    public enum PlayerState
    {
        Idle,
        Walking,
        Jumping
    }
}

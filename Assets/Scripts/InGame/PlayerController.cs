using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] private float jumpForce = 5f;
    private Rigidbody rb;
    private Vector3 playerScale;
    private PlayerState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerScale = transform.localScale;
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
                if (IsWalking())
                {
                    newState = PlayerState.Walking;
                }
                break;

            case PlayerState.Walking:
                if (!IsWalking())
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
    // private bool IsJumping()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
    //     {
    //         transform.DOScale(new Vector3(1, 2, 1), 0.1f)
    //             .OnComplete(() =>
    //             {
    //                 rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

    //                 DOVirtual.DelayedCall(0.3f, () =>
    //                 {
    //                     transform.DOScale(new Vector3(1, 1, 1), 0.1f);
    //                 });
    //             });

    //         return true;
    //     }
    //     return false;
    // }

    // private bool IsGrounded()
    // {
    //     return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    // }


    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 targetVelocity = new Vector3(moveX * moveSpeed, moveY * moveSpeed, 0);

        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, 0.2f);

        if (moveX != 0)
        {
            Move(moveX);
        }

    }


    //================================================================
    void Move(float moveX)
    {
        transform.DOMoveX(transform.position.x + moveX * moveSpeed, 0.5f)
                 .SetEase(Ease.OutBounce);
    }
    //=========================================================
    public enum PlayerState
    {
        Idle,
        Walking
    }
}

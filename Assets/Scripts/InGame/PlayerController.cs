using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // DOTween kütüphanesi eklenmeli

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] private float moveDuration = 0.2f; //animasyonlar için önemlidir ne kadar sürede tamamlanacağını belirliyoruz
    [SerializeField] private float jumpForce = 30f;
    private Rigidbody rb;
    private PlayerState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative; // Çarpışma tespitini artırıyoruz
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Daha yumuşak hareket sağlıyoruz 
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
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector2.down, 1.1f);
    }


    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");

        if (Mathf.Abs(moveX) > 0.1f)
        {
            float targetX = transform.position.x + (moveX * moveSpeed * Time.deltaTime);

            transform.DOMoveX(targetX, moveDuration)
                .SetEase(Ease.OutQuad);
        }
    }

    public enum PlayerState
    {
        Idle,
        Walking,
        Jumping
    }
}
